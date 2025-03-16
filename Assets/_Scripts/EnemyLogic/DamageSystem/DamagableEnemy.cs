using UnityEngine;

public class DamagableEnemy : Damagable
{
    EnemyBrain brain;
    public override void OnDamageTaken(float damage)
    {
        base.OnDamageTaken(damage);
        brain.animator.SetTrigger("Hit");

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
        
    }

    protected override void Update()
    {
        base.Update();
    }
}
