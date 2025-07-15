using UnityEngine;

public class FreezeEffectModifier : WeaponModifier
{


    private float length = 2f;

    public FreezeEffectModifier(float probability, float duration) : base(probability)
    {
        length = duration;
        name = "Freeze Modifier";
    }

    public override void Trigger(WeaponHolder holder, Damagable target, float damage, RaycastHit? hit = null)
    {
        DamagableEnemy enemy = target as DamagableEnemy;
        if (enemy == null) return;
        holder.ApplyEffectToEnemy(new Enemy_FreezeDebuff { Duration = length}, enemy);
        Debug.Log($"EnemyFreeze triggered: froze enemy for {length} Seconds.");
    }
}