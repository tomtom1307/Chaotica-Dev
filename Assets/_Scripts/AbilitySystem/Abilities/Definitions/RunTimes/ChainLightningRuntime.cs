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
                var anchor = GetAnchorFromGO(first);
                if (anchor != null)
                {
                    chainTargets.Add(anchor);            //  anchor, not first.transform
                    changed = true;
                    nextLinkAllowedAt = Time.time + config.LinkDelay;
                    lastPrimaryOkTime = Time.time;
                }
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
                    var anchor = GetAnchorFromGO(first);
                    if (anchor != null)
                    {
                        chainTargets[0] = anchor;            //  anchor, not root
                        changed = true;
                        lastPrimaryOkTime = Time.time;
                    }
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
        if (!from) return null;

        var hits = Physics.OverlapSphere(from.position, config.JumpRange, ~0, QueryTriggerInteraction.Ignore);

        float bestDist = float.MaxValue;
        Transform bestAnchor = null;

        foreach (var h in hits)
        {
            if (!h) continue;

            // always resolve to Damagable + anchor
            var anchor = GetAnchorFromGO(h.gameObject);
            if (!anchor) continue;

            // skip self/caster
            if (anchor.root == from.root) continue;
            if (anchor.root == transform.root) continue;

            // skip if this enemy (by root) is already in the chain
            bool already = false;
            for (int i = 0; i < chainTargets.Count; i++)
            {
                var t = chainTargets[i];
                if (t && t.root == anchor.root) { already = true; break; }
            }
            if (already) continue;

            // distance test
            float d = (anchor.position - from.position).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                bestAnchor = anchor;  // always anchor
            }
        }

        return bestAnchor;
    }


    private bool IsEnemy(Transform t)
    {
        var dmg = Damagable.CheckForDamagable(t ? t.gameObject : null);
        return dmg && dmg.CompareTag("Enemy");
    }

    private void RebuildChain(bool forceReacquireFirst)
    {
        chainTargets.Clear();

        var go = config.GetBestEnemy(config.JumpRange, config.MaxViewAngle, gameObject);
        var anchor = GetAnchorFromGO(go);
        if (anchor != null)
            chainTargets.Add(anchor);          
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
            var dmg = Damagable.CheckForDamagable(t.gameObject);
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

    private Transform GetAnchorFromGO(GameObject go)
    {
        if (!go) return null;
        var dmg = Damagable.CheckForDamagable(go);
        return dmg ? dmg.GetTargetPos() : null;
    }

    private Transform GetAnchorFromTransform(Transform t)
    {
        return t ? GetAnchorFromGO(t.gameObject) : null;
    }

    #endregion
}
