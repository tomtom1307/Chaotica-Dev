using UnityEngine;

[RequireComponent(typeof(EnemySoundSource))]
public class DamagableEnemy : Damagable
{
    EnemyBrain brain;
    EnemySoundSource soundSource;
    public GameObject hitParticleFX;
    public float InstantAgro = 5;
    public override void OnDamageTaken(float damage)
    {
        soundSource.PlaySound(EnemySoundSource.SoundType.TakeDamageBladed, 2);
        base.OnDamageTaken(damage);
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
                brain.actionHandler.StartActionOverride(brain.actionHandler.ChangeToPatrolState);
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
}
