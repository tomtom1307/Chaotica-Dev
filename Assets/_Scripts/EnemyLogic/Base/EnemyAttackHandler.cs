using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    [HideInInspector] public EnemyBrain brain;
    [HideInInspector] public EnemyAttack currentAttack;
    List<bool> groupDidDamage;
    public bool attacking;


    public void EnterAttack(EnemyAttack EA)
    {
        if (attacking) { return; }
        Debug.Log("Enter Attack!");
        currentAttack = EA;
        attacking = true;
        brain.animator.SetInteger("AT", currentAttack.attackAnimation);
        currentAttack.attackData.EnterAttack(this);
        if(currentAttack.attackData.doCollider) groupDidDamage = Enumerable.Repeat(false, currentAttack.colliderGroups.Count).ToList();

        brain.animator.SetBool("Attacking", attacking);
    }
    public void ExitAttack()
    {
        brain.animator.SetInteger("AT", 0);
        brain.animator.SetBool("Attacking", false);
        Invoke(nameof(AttackCooldownExit), 0.1f);
        currentAttack.attackData.ExitAttack(this);
    }

    private void AttackCooldownExit()
    {
        attacking = false;
        Debug.Log("FinishedAttack!");
    }

    public void DamagePlayer(PlayerHealth PH)
    {
        Debug.Log("PlayerWasDamaged!");
        currentAttack.attackData.DamageLogic(PH, this);
        PH.TakeDamage(currentAttack.attackData.damageValue);
    }

    public void DoRayCast()
    {
        Vector3 shootDir = (brain.perception.player.position + Vector3.up - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, shootDir, out hit, currentAttack.attackData.rayCastRange, currentAttack.attackData.whatIsPlayer))
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

    }

    public void RecieveColliderHitCallback(PlayerHealth ph, ColliderDetector col)
    {
        int count = 0;
        foreach (colliderGroup colGroup in currentAttack.colliderGroups)
        {
            if (colGroup.colliderList.Contains(col) && !groupDidDamage[count])
            {
                Debug.Log("Attack group has not done damage yet!");
                DamagePlayer(ph); // Damage player
                groupDidDamage[count] = true; // Count group as having done damage
            }
            else Debug.Log("Ignored extra collider trigger.");
                count++;
        }
    }
    public void DisableColliderGroup(int colliderGroupIndex)
    {
        List<ColliderDetector> colliderGroupList = currentAttack.colliderGroups[colliderGroupIndex].colliderList;
        foreach(ColliderDetector col in colliderGroupList)
        {
            col.DisableCollider();
            col.OnDetectCallback -= RecieveColliderHitCallback;
        }

    }

    // Remove or keep?
    private IEnumerator DisableColliderAfterTime(float detectionTime, ColliderDetector col)
    {
        yield return new WaitForSeconds(detectionTime);
        col.DisableCollider();
        col.OnDetectCallback -= RecieveColliderHitCallback; 
    }


}
