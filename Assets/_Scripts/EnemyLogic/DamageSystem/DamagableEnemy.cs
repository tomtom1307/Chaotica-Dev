using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(EnemySoundSource))]
public class DamagableEnemy : Damagable
{
    [HideInInspector] public EnemyBrain brain;
    EnemySoundSource soundSource;
    public GameObject hitParticleFX;
    public float InstantAgro = 5;

    [HideInInspector] public Vector3 moveDirection;
    public override void OnDamageTaken(float damage, Color col)
    {
        soundSource.PlaySound(EnemySoundSource.SoundType.TakeDamageBladed, 2);
        base.OnDamageTaken(damage, col);
        brain.animator.SetTrigger("Hit");
        if(brain.stateMachine.CurrentEnemyState == brain.idleState)
        {
            if(Vector3.Distance(transform.position, brain.perception.player.position) < InstantAgro)
            {
                if (brain.perception.DI == null)
                {
                    brain.perception.DI = HUDController.instance.TriggerDetectionMeter(brain);
                    brain.perception.DetectionMeter = 1;
                    brain.perception.LSP_time = 0;
                }
                brain.Agro();
                
                return;
            }
            else
            {
                brain.actionHandler.StartActionOverride(brain.actionHandler.SearchNearby);
            }
        }
        if (hitParticleFX != null)
        {
            var ps = Instantiate(hitParticleFX, transform.position + spawnOffset, Quaternion.LookRotation(brain.perception.player.position - transform.position));


        }
        
        if ( DCM != null)
        {
            DCM.ChangeCrack(1-Health / MaxHealth);
        } 
    }


    public override void Die()
    {
        brain.animator.enabled = false;
        brain.attackHandler.DisableColliderGroup(0);
        brain.NotifySpawner();
        GameManager.instance.EnemyKilled();
        base.Die();
    }

    DamageCrackingManager DCM;
    protected override void Start()
    {
        
        base.Start();
        brain = GetComponent<EnemyBrain>();
        soundSource = GetComponent<EnemySoundSource>();

        DCM = GetComponent<DamageCrackingManager>();
        if (DCM == null)
        {
            DCM = GetComponentInChildren<DamageCrackingManager>();
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    
    public GameObject GetClosestEnemy(float Radius)
    {
        List<Collider> enemiesInRange = new List<Collider>();
        enemiesInRange = Physics.OverlapSphere(transform.position, Radius, gameObject.layer).ToList();

        if (enemiesInRange.Count > 0)
        {
            GameObject bestTarget = null;
            float closestDistance = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach (Collider enemy in enemiesInRange)
            {
                Vector3 directionToTarget = enemy.transform.position - currentPosition;
                float distToTarget = directionToTarget.sqrMagnitude;

                if (distToTarget < closestDistance)
                {
                    closestDistance = distToTarget;
                    bestTarget = enemy.gameObject;
                }
            }
            return bestTarget;
        }
        else
        {
            return null;
        }

    }

}
