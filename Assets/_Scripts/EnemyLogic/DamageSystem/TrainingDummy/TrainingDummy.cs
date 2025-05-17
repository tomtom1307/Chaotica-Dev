using DG.Tweening;
using UnityEngine;

public class TrainingDummy : Damagable
{
    Animator animator;
    public override void Die()
    {
        Health = MaxHealth;
    }

    public override void OnDamageTaken(float damage, Color col)
    {
        base.OnDamageTaken(damage, col);
        Debug.Log("Is Called");
        animator.SetTrigger("Hit");
        
    }

    public override void TakeDamage(float Damage, Vector3 pos, Vector3 normal, bool isCrit)
    {
        base.TakeDamage(Damage, pos, normal, isCrit);
    }

    public override void TakeDamage(float damage, bool HitFX = true, bool Crit = false)
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
