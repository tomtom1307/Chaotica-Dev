using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EAI_AttackHandler : MonoBehaviour
{
    EnemyContext ctx;
    [HideInInspector] public EAI_AttackSO_Base currentAttack;
    public List<colliderGroup> ColliderGroups;
    [HideInInspector] public EnemyVFXHandler vfxHandler;
    List<bool> groupDidDamage;
    public bool attacking;

    Rigidbody rb;
    Transform player;
    Rigidbody _playerRb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }


    public void Init(EnemyContext ctx)
    {
        this.ctx = ctx;
        player = GameManager.instance.player;
        _playerRb = player.GetComponent<Rigidbody>();
        vfxHandler = GetComponentInChildren<EnemyVFXHandler>();
    }

    Vector3 TargetPos;

    private void Update()
    {

    }

    public void EnterAttack()
    {

    }

    public void ExitAttack()
    {

    }

    private void AttackCooldownExit()
    {
        
    }

    private void DamagePlayer(PlayerHealth ph)
    {
        float Damage = CalculateDamage();
        ph.TakeDamage(Damage, ctx.Health, currentAttack.Paryable, currentAttack.Blockable);
    }

    public float CalculateDamage()
    {
        return currentAttack.Damage * ctx.cfg.BaseDamage;
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
        if (currentAttack == null) return;
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
}


