using System.Collections;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    [HideInInspector] public EnemyBrain brain;
    [HideInInspector] public EnemyAttack currentAttack;
    public bool attacking;


    public void EnterAttack(EnemyAttack EA)
    {
        if (attacking) { return; }
        Debug.Log("Enter Attack!");
        currentAttack = EA;
        attacking = true;
        brain.animator.SetInteger("AT", currentAttack.attackType);
        currentAttack.attackData.EnterAttack(this);

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
        currentAttack.attackData.DamageLogic(PH , this);
        PH.TakeDamage(10);

    }

    public void DoRayCast() 
    {
        Vector3 shootDir = (brain.perception.player.position+Vector3.up - transform.position).normalized;
        RaycastHit hit; 
        if(Physics.Raycast(transform.position, shootDir, out hit, currentAttack.attackData.rayCastRange, currentAttack.attackData.whatIsPlayer))
        {
            PlayerHealth PH = hit.collider.gameObject.GetComponent<PlayerHealth>();
            DamagePlayer(PH);
        }
        
    
    }

    public void DoProjectile() { }

    public void DoColliderCheck() {
        foreach (var colliderGroup in currentAttack.colliderGroups)
        {
            float DetectTime = colliderGroup.DetectTime;
            colliderGroup.collider.TriggerDetection(DetectTime);
            colliderGroup.collider.OnDetectCallback += RecieveColliderHitCallback;
            StartCoroutine(DisableColliderAfterTime(DetectTime, colliderGroup.collider));
        }
    
    }

    public void RecieveColliderHitCallback(PlayerHealth ph, ColliderDetector col)
    {
        
        DamagePlayer(ph);
        
    }

    private IEnumerator DisableColliderAfterTime(float detectionTime, ColliderDetector col)
    {
        yield return new WaitForSeconds(detectionTime);
        col.DisableCollider();
        col.OnDetectCallback -= RecieveColliderHitCallback; 
    }


}
