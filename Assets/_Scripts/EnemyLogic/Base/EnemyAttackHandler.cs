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
    
    Rigidbody rb;
    Transform player;
    Rigidbody _playerRb;

    float AttackTimer;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }


    public void Init(EnemyBrain brain)
    {
        this.brain = brain;
        player = GameManager.instance.player;
        _playerRb = player.GetComponent<Rigidbody>();
    }


    public void EnterAttack(EnemyAttack EA)
    {
        if (attacking || AttackTimer <= brain.TimeBetweenAttacks) { return; }
        currentAttack = EA;
        
        attacking = true;

        if (currentAttack.attackData.rootMotion)
        {
            Debug.Log("enabled root motion and disabled navmeshagent.");
            brain.navMesh.isStopped = true;
            
            //brain.navMesh.speed = 0;
            //brain.navMesh.enabled = false;
        }
        else Debug.Log("rootmotion in data is set to false.");
            brain.animator.SetInteger("AT", currentAttack.attackAnimation);
        currentAttack.attackData.EnterAttack(this);
        if(currentAttack.attackData.doCollider) groupDidDamage = Enumerable.Repeat(false, currentAttack.colliderGroups.Count).ToList();

        brain.animator.SetBool("Attacking", attacking);
    }



    private void OnAnimatorMove()
    {
        if (!enabled) return;
        Vector3 newPos = transform.position + brain.animator.deltaPosition;
        transform.position = newPos;
    }

    private void Update()
    {
        if (attacking)
        {
            currentAttack.attackData.AttackUpdate(this);
        }
        else
        {
            AttackTimer += Time.deltaTime;
        }
        AimDirection = (player.position + Vector3.up - brain.LookDirectionTransform.position).normalized;
        AimDirection += -0.1f * (1 - brain.Accuracy) * _playerRb.linearVelocity;

    }

    public void ExitAttack()
    {
        
        if (currentAttack.attackData.rootMotion)
        {
            brain.navMesh.isStopped = false;
        }
        brain.animator.SetInteger("AT", 0);
        brain.animator.SetBool("Attacking", false);
        Invoke(nameof(AttackCooldownExit), Mathf.Clamp(currentAttack.attackData.AttackCooldown, 0.1f, 10000000));
        currentAttack.attackData.ExitAttack(this);
    }

    private void AttackCooldownExit()
    {
        attacking = false;
    }

    public void DamagePlayer(PlayerHealth PH)
    {
        currentAttack.attackData.DamageLogic(PH, this);
        PH.TakeDamage(currentAttack.attackData.damageValue);
    }
    [HideInInspector] public Vector3 AimDirection;
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

    }

    public void RecieveColliderHitCallback(PlayerHealth ph, ColliderDetector col)
    {
        int count = 0;
        foreach (colliderGroup colGroup in currentAttack.colliderGroups)
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
