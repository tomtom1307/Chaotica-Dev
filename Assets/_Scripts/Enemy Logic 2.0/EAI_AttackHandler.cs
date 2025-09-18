using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TRTools.floatOP;

public class EAI_AttackHandler : MonoBehaviour
{
    EnemyContext ctx;
    [HideInInspector] public EAI_AttackSO_Base currentAttack;
    public List<colliderGroup> ColliderGroups;
    [HideInInspector] public EnemyVFXHandler vfxHandler;
    [HideInInspector] public List<bool> groupDidDamage;
    public bool attacking;

    Rigidbody rb;
    Transform player;
    Rigidbody _playerRb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ctx = GetComponent<EnemyContext>(); 
    }


    public void Init(EnemyContext ctx)
    {
        this.ctx = ctx;
        player = GameManager.instance.player;
        _playerRb = player.GetComponent<Rigidbody>();
        vfxHandler = GetComponentInChildren<EnemyVFXHandler>();
    }

    Vector3 TargetPos => ctx.bb.target.position;

    private void Update()
    {
        ctx.bb.AttackAvailable = AttackAvailable();

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            AttackExit();
        }
        
    }


    private void DamagePlayer(PlayerHealth ph)
    {
        float Damage = CalculateDamage();
        ph.TakeDamage(Damage, ctx.Health, currentAttack.Paryable, currentAttack.Blockable);
        KnockbackPlayer(ph.gameObject);
    }


    public void KnockbackPlayer(GameObject player)
    {
        float mag = currentAttack.Knockback;
        Vector3 dir = TRTools.VecOp.Direction(transform.position, TargetPos);
        dir.y = 0;
        Vector3 Force = mag * dir.normalized;
        player.GetComponent<IKnockbackable>().GetKnockedBack(Force);
    }

    public float CalculateDamage()
    {
        return currentAttack.Damage * ctx.cfg.BaseDamage * 0.01f;
    }

    public void HandleAttackEnter(AbilityEntry abilityEntry)
    {
        ctx.anim.PlayAttack(abilityEntry.Animation_Index);
        ctx.anim.ApplyRootMotion(abilityEntry.Rootmotion);
        ctx.attackHandler.InitColliders();
        currentAttack = abilityEntry.Ability as EAI_AttackSO_Base;
    }


    public void InitColliders()
    {
        groupDidDamage = Enumerable.Repeat(false, ColliderGroups.Count).ToList();
    }
    public void AttackExit()
    {
        StartCoroutine(AttackCooldown(0.5f));
        ctx.aimer.ResetSpeedToDefault();
        ctx.anim.ResetAttackAnim();
    }

    public IEnumerator AttackCooldown(float length)
    {
        ctx.bb.attack_State = EAI_Blackboard.AttackState.cooldown;
        yield return new WaitForSeconds(length);
        ctx.bb.attack_State = EAI_Blackboard.AttackState.ready;
    }


    [HideInInspector] public Vector3 AimDirection;
    /*
    public void DoRayCast()
    {

        RaycastHit hit;

        Debug.DrawRay(transform.position, AimDirection, Color.yellow, 5);
        if (Physics.Raycast(brain.LookDirectionTransform.position, AimDirection.normalized, out hit, currentAttack.attackData.rayCastRange, currentAttack.attackData.whatIsPlayer))
        {
            PlayerHealth PH = hit.collider.gameObject.GetComponent<PlayerHealth>();
            DamagePlayer(PH);
        }


    }

    public void DoProjectile() { }

    public void DoColliderCheck(int colliderGroupIndex)
    {
        if (currentAttack.attackData.doCollider)
        {
            List<ColliderDetector> colliderGroupList = currentAttack.colliderGroups[colliderGroupIndex].colliderList;
            foreach (ColliderDetector col in colliderGroupList)
            {
                col.TriggerDetection();
                col.OnDetectCallback += RecieveColliderHitCallback;
            }
        }
        else Debug.LogError("You must set the attack SO DoCollider bool to true to initialize the necesary lists.");

    }*/

    public void DoColliderCheck(int colliderGroupIndex)
    {
        List<ColliderDetector> colliderGroupList = ctx.attackHandler.ColliderGroups[colliderGroupIndex].colliderList;
        foreach (ColliderDetector col in colliderGroupList)
        {
            col.TriggerDetection();
            col.OnDetectCallback += RecieveColliderHitCallback;
        }

    }


    public void RecieveColliderHitCallback(PlayerHealth ph, ColliderDetector col)
    {
        int count = 0;
        foreach (colliderGroup colGroup in ColliderGroups)
        {
            if (colGroup.colliderList.Contains(col) && !groupDidDamage[count])
            {
                DamagePlayer(ph); // Damage player
                groupDidDamage[count] = true; // Count group as having done damage
            }
            count++;
        }
    }
    

    public void DisableColliderGroup(int colliderGroupIndex)
    {
        if (ColliderGroups.Count < colliderGroupIndex + 1) return;
        List<ColliderDetector> colliderGroupList = ColliderGroups[colliderGroupIndex].colliderList;
        foreach (ColliderDetector col in colliderGroupList)
        {
            col.DisableCollider();
            col.OnDetectCallback -= RecieveColliderHitCallback;
        }
    }

    public void DisableAllColliderGroup()
    {
        for (int i = 0; i < ColliderGroups.Count; i++)
        {
            DisableColliderGroup(i);
        }
    }

    // Remove or keep?
    private IEnumerator DisableColliderAfterTime(float detectionTime, ColliderDetector col)
    {
        yield return new WaitForSeconds(detectionTime);
        col.DisableCollider();
        col.OnDetectCallback -= RecieveColliderHitCallback;
    }

    public void SpawnVFX(int index)
    {

        if (vfxHandler == null)
        {
            Debug.LogError("There is no VFXHandler on this object!");
            return;
        }
        GameObject VFX;
        if (currentAttack.VFXs[index].isParentedToHolder)
        {

            VFX = Instantiate(currentAttack.VFXs[index].Prefab, vfxHandler.transform);
            VFX.transform.localPosition = Vector3.zero;
        }
        else
        {
            VFX = Instantiate(currentAttack.VFXs[index].Prefab, vfxHandler.transform.position, vfxHandler.transform.rotation);
            VFX.transform.localScale = vfxHandler.transform.localScale;
        }


        vfxHandler.SpawnedVFX.Add(VFX);
    }

    public void DestroyVFX()
    {
        foreach (GameObject vfx in vfxHandler.SpawnedVFX)
        {
            vfxHandler.SpawnedVFX.Remove(vfx);
            Destroy(vfx);
        }
    }


    public bool AttackAvailable()
    {
        List<AbilityEntry> attacks = new List<AbilityEntry>(ctx.cfg.Attacks);
        foreach (var attack in attacks)
        {
            if (CanDoAttack(attack)) return true;
        }
        return false;
    }

    public bool CanDoAttack(AbilityEntry a)
    {
        bool _inRange = InRange(ctx.bb.distanceToTarget, a.MinRange, a.MaxRange);
        bool _Los = a.los ? ctx.bb.hasLOS : true;
        bool _enabled = a.Enabled;
        return _inRange && _enabled && _Los;
    }
}


