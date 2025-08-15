using System;
using UnityEngine;

[Serializable]
public class LifestealEffectModifier : WeaponModifier
{


    [SerializeField] private float stealPercent = 0.5f;

    public LifestealEffectModifier(float probability,float Steal_percent) : base(probability)
    {
        stealPercent = Steal_percent;
        name = "lifeSteal";
    }

    public LifestealEffectModifier()
    {
        name = "lifeSteal";
    }

    public override void Trigger(WeaponHolder holder, Damagable target, float damage, RaycastHit? hit = null)
    {
        float healAmount = damage * stealPercent / 100;
        PlayerHealth.instance.Heal(healAmount);
        Debug.Log($"Lifesteal triggered: healed {healAmount} HP.");
    }
}