using UnityEngine;

public class Enemy_FreezeDebuff : statusEffectBase
{
 
    protected override void onApply(GameObject Object)
    {
        enemy = Object.GetComponent<EnemyBrain>();

        if (enemy == null)
        {
            Debug.LogError("FreezeStatus: Could not find enemy");
            return;
        }

        enemy.Freeze(true);
    }

    protected override void OnExpire(GameObject Object)
    {
        if (enemy == null)
        {
            Debug.LogError("FreezeStatus: Could not find enemy");
            return;
        }

        enemy.Freeze(false);
    }
}
