using UnityEngine;

[RequireComponent(typeof(EnemySoundSource))]
public class DamagableEnemy : Damagable
{
    EnemyBrain brain;
    EnemySoundSource soundSource;
    public GameObject hitParticleFX;
    public override void OnDamageTaken(float damage)
    {
        base.OnDamageTaken(damage);
        brain.animator.SetTrigger("Hit");
        if(hitParticleFX != null)
        {
            var ps = Instantiate(hitParticleFX, transform.position + spawnOffset, Quaternion.LookRotation(brain.perception.player.position - transform.position));


        }
        soundSource.PlaySound(EnemySoundSource.SoundType.TakeDamageBladed, 2);
    }


    public override void Die()
    {
        GameManager.instance.EnemyKilled();
        base.Die();
    }

    protected override void Start()
    {
        base.Start();
        brain = GetComponent<EnemyBrain>();
        soundSource = GetComponent<EnemySoundSource>();

    }

    protected override void Update()
    {
        base.Update();
    }
}
