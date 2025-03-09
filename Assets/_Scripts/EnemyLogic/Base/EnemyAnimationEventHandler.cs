using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    public EnemyAttackHandler attackHandler;

    //THIS SCRIPT EXISTS TO SIMPLIFY THE UNITY UI WHEN DOING ANIMATION EVENTS


    public void ExitAttackCall() => attackHandler.ExitAttack();

    public void StartAttack(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.Raycast:
                attackHandler.DoRayCast();
                break;
            case AttackType.Collider:
                attackHandler.DoColliderCheck(0); // For some reason function does not show when StartAttack() takes an integer parameter. Are only 2 parameters allowed?
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
    public void DisableCollider(int colliderGroup) => attackHandler.DisableColliderGroup(colliderGroup);
}
