using UnityEngine;

public class LifestealEffectModifier : WeaponModifier
{


    private float stealPercent = 0.5f;

    public LifestealEffectModifier(float probability,float Steal_percent) : base(probability)
    {
        stealPercent = Steal_percent;
        name = "lifeSteal";
    }

    public override void Trigger(WeaponHolder holder, Damagable target, float damage, RaycastHit? hit = null)
    {
        float healAmount = damage * stealPercent / 100;
        PlayerHealth.instance.Heal(healAmount);
        Debug.Log($"Lifesteal triggered: healed {healAmount} HP.");
    }
}