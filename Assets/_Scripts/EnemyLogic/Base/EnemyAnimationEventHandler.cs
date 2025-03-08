using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    public EnemyAttackHandler attackHandler;

    //THIS SCRIPT EXISTS TO SIMPLIFY THE UNITY UI WHEN DOING ANIMATION EVENTS


    public void ExitAttack()
    {
        
    }

    public void DoAttack(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.Raycast:
                attackHandler.DoRayCast();
                break;
            case AttackType.Collider:
                attackHandler.DoColliderCheck();
                break;
            case AttackType.Projectile:
                attackHandler.DoProjectile();
                break;
            default:
                break;
        }
    }

    public enum AttackType
    {
        Raycast,
        Collider,
        Projectile
    }
}
