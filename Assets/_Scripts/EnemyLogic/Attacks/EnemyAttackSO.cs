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

    [Range(0.0f, 1.0f)]
    [Tooltip("Changes how velocity dependent raycast is:" +
        " {1 being raycast will hit regardless of player speed to " +
        "0 being will certainly miss when player moving}")]
    public float Accuracy; // Ranges from 0 to 1

    public virtual void EnterAttack(EnemyAttackHandler attackHandler)
    {
    }
    public virtual void ExitAttack(EnemyAttackHandler attackHandler) 
    {
    }


    public virtual void DamageLogic(PlayerHealth ph,EnemyAttackHandler attackHandler) {
        if (KnockBack)
        {
            ph.GetComponent<Rigidbody>().AddForce(KnockBackAmount * attackHandler.transform.forward);
        }
    }
}
