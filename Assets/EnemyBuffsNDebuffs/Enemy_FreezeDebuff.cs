using UnityEngine;

public class Enemy_FreezeDebuff : statusEffectBase
{
 
    protected override void onApply(GameObject Object)
    {
        enemy = Object.GetComponent<EnemyBrain>();

        if (enemy == null)
        {
            return;
        }

        enemy.Freeze(true);
    }

    protected override void OnExpire(GameObject Object)
    {
        if (enemy == null)
        {
            return;
        }

        enemy.Freeze(false);
    }
}
