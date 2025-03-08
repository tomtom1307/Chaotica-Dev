using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackSO", menuName = "Scriptable Objects/EnemyAttackSO")]
public class EnemyAttackSO : ScriptableObject
{
    bool rootMotion;
    bool interruptable;
    LayerMask whatIsPlayer;
    [Header("Collider")]
    bool doCollider;
    float damageValue;

    [Header("Projectile")]
    bool doProjectile;
    bool considerGravity;
    GameObject Projectile;
    float projectileDamage;
    float projectileSpeed;
    float projectileSpread;
    float projectileAmount;

    [Header("RayCast")]
    bool doRayCast;
    float rayCastDamage;
    float rayCastRange;

    public virtual void EnterAttack(EnemyAttackHandler attackHandler)
    {
    }
    public virtual void ExitAttack(EnemyAttackHandler attackHandler) 
    {
    }
}
