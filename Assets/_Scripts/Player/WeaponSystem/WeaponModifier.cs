
using System;
using UnityEngine;

[Serializable]
public abstract class WeaponModifier
{
    public string name; 
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




