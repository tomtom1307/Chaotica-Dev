using UnityEngine;

[RequireComponent(typeof(EnemySoundSource))]
public class DamageableBrainLessEnemy : Damagable
{
    
    EnemySoundSource soundSource;
    public override void OnDamageTaken(float damage, Color col)
    {
        soundSource.PlaySound(EnemySoundSource.SoundType.TakeDamageBladed, 2);
        base.OnDamageTaken(damage, col);
        if (DCM != null)
        {
            DCM.ChangeCrack(1 - Health / MaxHealth);
        }
    }


    public override void Die()
    {
        GameManager.instance.EnemyKilled();
        base.Die();
    }
    Transform player;
    DamageCrackingManager DCM;
    protected override void Start()
    {

        base.Start();
        player = GameManager.instance.player;
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
