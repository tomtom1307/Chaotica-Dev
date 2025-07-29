using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    public EnemyAttackHandler attackHandler;

    //THIS SCRIPT EXISTS TO SIMPLIFY THE UNITY UI WHEN DOING ANIMATION EVENTS
    EnemySoundSource soundSource;
    private void Start()
    {
        soundSource = GetComponent<EnemySoundSource>();
    }


    public void ExitAttackCall() => attackHandler.ExitAttack();

    public void DoNonColliderAttack(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.Raycast:
                attackHandler.DoRayCast();
                break;
            case AttackType.Projectile:
                attackHandler.DoProjectile();
                break;
            default:
                break;
        }
    }
    public void StartColliderAttack(int colliderGroupIndex) => attackHandler.DoColliderCheck(colliderGroupIndex);

    public enum AttackType
    {
        Raycast,
        Projectile
    }
    public void DisableCollider(int colliderGroup) => attackHandler.DisableColliderGroup(colliderGroup);

    public void PlaySound(EnemySoundSource.SoundType soundType)
    {
        soundSource.PlaySound(soundType, -1);
    }

    public void SpawnVFX(int i)
    {
        print("SpawnVFX");
        attackHandler.SpawnVFX(i);
    }
    public void DestroyVFX() => attackHandler.DestroyVFX();
}
