using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackSO", menuName = "AttackSO/EnemyAttackSO")]
public class EnemyAttackSO : ScriptableObject
{
    public bool rootMotion;
    public bool interruptable;
    public float AttackCooldown;

    public float KnockBackAmount;
    public bool KnockBack;


    public LayerMask whatIsPlayer;
    [Header("Collider")]
    public bool doCollider;
    public float damageValue;

    [Header("Projectile")]
    public bool doProjectile;
    public bool considerGravity;
    public GameObject Projectile;
    public float projectileDamage;
    public float projectileSpeed;
    public float projectileSpread;
    public float projectileAmount;

    [Header("RayCast")]
    public bool doRayCast;
    public float rayCastDamage;
    public float rayCastRange;

    
    

    public virtual void EnterAttack(EnemyAttackHandler attackHandler)
    {
    }
    public virtual void ExitAttack(EnemyAttackHandler attackHandler) 
    {
    }

    public virtual void AttackUpdate(EnemyAttackHandler attackHandler)
    {
        
    }

    public virtual void DamageLogic(PlayerHealth ph,EnemyAttackHandler attackHandler) {
        if (KnockBack)
        {
            ph.GetComponent<Rigidbody>().AddForce(KnockBackAmount * attackHandler.transform.forward);
        }
    }
}
