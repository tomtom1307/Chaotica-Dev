using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningRuntime : MonoBehaviour
{
    private ChainLightning config;
    private AbilityHolder holder;
    private GameObject lightningPrefab;

    private readonly List<Transform> chainTargets = new();
    private readonly List<GameObject> spawnedLines = new();

    private Coroutine loop;
    private WaitForSeconds refreshWait;
    private const float RefreshRate = 0.03f;    
    private const float DamageTick = 0.1f;       

    private float damageAccumulator;
    private float nextLinkAllowedAt = 0f;

    // --- stickiness knobs ---
    [SerializeField] private float primaryGrace = 0.8f;      // seconds you can look away a bit
    [SerializeField] private float retentionAngleBonus = 10f; // extra degrees allowed after lock

    private float lastPrimaryOkTime = -1f;

    private bool IsPrimaryStillOk(Transform t)
    {
        if (t == null) return false;

        var to = t.position - transform.position;
        if (to.sqrMagnitude > config.JumpRange * config.JumpRange) return false;

        // wider angle once we're already locked on
        float limit = config.MaxViewAngle + retentionAngleBonus;
        float ang = Vector3.Angle(transform.forward, to);

        if (ang <= limit)
        {
            lastPrimaryOkTime = Time.time; // refresh "seen recently"
            return true;
        }

        // short grace period even if beyond the wider angle
        return (Time.time - lastPrimaryOkTime) <= primaryGrace;
    }


    public void Init(ChainLightning config, AbilityHolder holder, GameObject lightningPrefab)
    {
        this.config = config;
        this.holder = holder;
        this.lightningPrefab = lightningPrefab;
        refreshWait = new WaitForSeconds(RefreshRate);
    }

    void LateUpdate()
    {
        // run after movement/camera updates
        if (spawnedLines.Count > 0 && holder != null && holder.IsAbilityActive)
        {
            UpdateLines();
        }
    }

    public void Begin()
    {
        if (loop != null) StopCoroutine(loop);

        RebuildChain(forceReacquireFirst: true);     // only picks the first target
        RebuildLines();

        nextLinkAllowedAt = Time.time + config.LinkDelay;

        loop = StartCoroutine(UpdateLoop());
    }

    public void End()
    {
        if (loop != null)
        {
            StopCoroutine(loop);
            loop = null;
        }

        CleanupLines();
        chainTargets.Clear();
    }

    private IEnumerator UpdateLoop()
    {
        float tickTimer = 0f;
        while (holder != null && holder.IsAbilityActive)
        {
            MaintainChain();

            tickTimer += Time.deltaTime;            // instead of += RefreshRate
            if (tickTimer >= DamageTick)
            {
                ApplyDamage(config.DPS * tickTimer);
                tickTimer = 0f;
            }
            yield return refreshWait;
        }



        // Safety cleanup if ability ends abruptly
        End();
    }

    #region Chain & Targeting

    private void MaintainChain()
    {
        bool changed = false;
        bool removedLink = false;
        int targetCap = Mathf.Max(1, config.MaxEnemiesInChain);

        // 1) Ensure / reacquire primary
        if (chainTargets.Count == 0)
        {
            var first = config.GetBestEnemy(config.JumpRange, config.MaxViewAngle, gameObject);
            if (first != null)
            {
                chainTargets.Add(first.transform);
                changed = true;

                // only when we go from 0 -> 1 do we delay the next hop
                nextLinkAllowedAt = Time.time + config.LinkDelay;
                lastPrimaryOkTime = Time.time;
            }
        }
        else // we have a primary, check stickiness
        {
            if (!IsPrimaryStillOk(chainTargets[0]))
            {
                // try to reacquire, but DO NOT reset hop timer if we already had a primary
                var first = config.GetBestEnemy(config.JumpRange, config.MaxViewAngle, gameObject);
                if (first == null)
                {
                    chainTargets.Clear();
                    changed = true;
                    removedLink = true;
                }
                else if (first.transform != chainTargets[0])
                {
                    chainTargets[0] = first.transform;
                    changed = true;
                    lastPrimaryOkTime = Time.time;
                    // note: don't touch nextLinkAllowedAt here
                }
            }
        }

        // 2) Add at most ONE extra link when delay elapsed
        if (chainTargets.Count > 0 && chainTargets.Count < targetCap && Time.time >= nextLinkAllowedAt)
        {
            var tail = chainTargets[chainTargets.Count - 1];
            var next = FindNextLink(tail);
            if (next != null)
            {
                chainTargets.Add(next);
                changed = true;
                nextLinkAllowedAt = Time.time + config.LinkDelay; // schedule next hop
            }
        }

        // 3) Prune invalid links beyond the primary (range broken, died, etc.)
        for (int i = chainTargets.Count - 1; i >= 1; i--)
        {
            var curr = chainTargets[i];
            var prev = chainTargets[i - 1];
            if (curr == null || prev == null ||
                (curr.position - prev.position).sqrMagnitude > config.JumpRange * config.JumpRange)
            {
                chainTargets.RemoveAt(i);
                changed = true;
                removedLink = true;
            }
        }

        // 4) Rebuild VFX if structure changed
        if (changed)
        {
            RebuildLines();

            // Small debounce only if we LOST links; don't penalize successful growth
            if (removedLink && chainTargets.Count > 0)
                nextLinkAllowedAt = Mathf.Max(nextLinkAllowedAt, Time.time + 0.03f);
        }
    }



    private bool IsValidPrimary(Transform t)
    {
        if (t == null) return false;
        var to = t.position - transform.position;
        if (to.sqrMagnitude > config.JumpRange * config.JumpRange) return false;
        var ang = Vector3.Angle(transform.forward, to);
        return ang <= config.MaxViewAngle + 0.1f; // small tolerance
    }

    private Transform FindNextLink(Transform from)
    {
        // Simple nearest-in-sphere not already in chain
        // Replace with your own enemy query / layer mask / LOS checks as needed.
        var hits = Physics.OverlapSphere(from.position, config.JumpRange);
        float bestDist = float.MaxValue;
        Transform best = null;

        foreach (var h in hits)
        {
            var tr = h.transform;
            if (tr == from) continue;
            if (tr == transform) continue; // skip self
            if (chainTargets.Contains(tr)) continue;
            if (!IsEnemy(tr)) continue;

            float d = (tr.position - from.position).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                best = tr;
            }
        }

        return best;
    }

    private bool IsEnemy(Transform t)
    {
        // Customize: tag, layer, component check, etc.
        return t.CompareTag("Enemy");
    }

    private void RebuildChain(bool forceReacquireFirst)
    {
        chainTargets.Clear();

        Transform first = null;
        var go = config.GetBestEnemy(config.JumpRange, config.MaxViewAngle, gameObject);
        if (go != null) first = go.transform;

        if (first != null) chainTargets.Add(first);
    }


    #endregion

    #region Lines & Damage

    private void RebuildLines()
    {
        int needed = Mathf.Max(0, chainTargets.Count);

        
        while (spawnedLines.Count < needed)
        {
            if (lightningPrefab == null) break;
            var go = Instantiate(lightningPrefab, transform); // parent under caster for tidy hierarchy
            spawnedLines.Add(go);
        }
        while (spawnedLines.Count > needed)
        {
            var last = spawnedLines[spawnedLines.Count - 1];
            spawnedLines.RemoveAt(spawnedLines.Count - 1);
            if (last) Destroy(last);
        }

        // Initial position set immediately
        UpdateLines();
    }

    private void UpdateLines()
    {
        for (int i = 0; i < spawnedLines.Count; i++)
        {
            var lineGO = spawnedLines[i];
            if (!lineGO) continue;

            Transform a = (i == 0) ? transform : chainTargets[i - 1];
            Transform b = chainTargets[i];

            
            var lr = lineGO.GetComponent<LightningLR>();
            if (lr != null && a != null && b != null)
            {
                lr.SetPosition(a, b);
            }
        }
    }

    private void ApplyDamage(float amountThisTick)
    {
        foreach (var t in chainTargets)
        {
            if (t == null) continue;

            // Adapt to your damage system:
            // IDamageable, Health, or SendMessage
            var dmg = t.GetComponent<Damagable>();
            if (dmg != null)
            {
                dmg.TakeDamage(amountThisTick);
            }
            else
            {
                // Example fallback:
                t.SendMessage("ApplyDamage", amountThisTick, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void CleanupLines()
    {
        for (int i = 0; i < spawnedLines.Count; i++)
        {
            if (spawnedLines[i]) Destroy(spawnedLines[i]);
        }
        spawnedLines.Clear();
    }

    #endregion
}
