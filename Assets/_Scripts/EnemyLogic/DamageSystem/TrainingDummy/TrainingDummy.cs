using DG.Tweening;
using UnityEngine;

public class TrainingDummy : Damagable
{
    Animator animator;
    public override void Die()
    {
        Health = MaxHealth;
    }

    public override void OnDamageTaken(float damage)
    {
        base.OnDamageTaken(damage);
        Debug.Log("Is Called");
        animator.SetTrigger("Hit");
        
    }

    public override void TakeDamage(float Damage, Vector3 pos, Vector3 normal)
    {
        base.TakeDamage(Damage, pos, normal);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }


    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

    }

    protected override void Update()
    {
        base.Update();
    }
}
