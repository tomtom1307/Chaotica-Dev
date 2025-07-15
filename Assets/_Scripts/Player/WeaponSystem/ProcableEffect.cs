
using System;
using UnityEngine;

[Serializable]
public abstract class WeaponModifier
{
    float probability;
    public WeaponDataSO.Rarity rarity;


    public WeaponModifier(float prob)
    {
        probability = prob;
    }

    public virtual void GetProbability()
    {

    }

    public bool TryProc()
    {
        return UnityEngine.Random.value < probability;
    }

    public abstract void Trigger(WeaponHolder W, Damagable target, float damage, RaycastHit? hit = null);


}


public class LifestealEffect : WeaponModifier
{
    private float stealPercent = 0.5f;

    public LifestealEffect(float probability, float percent) : base(probability)
    {
        stealPercent = percent;
    }

    public override void Trigger(WeaponHolder holder, Damagable target, float damage, RaycastHit? hit = null)
    {
        float healAmount = damage * stealPercent/100;
        PlayerHealth.instance.Heal(healAmount);
        Debug.Log($"Lifesteal triggered: healed {healAmount} HP.");
    }
}