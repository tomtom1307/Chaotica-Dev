
using System;
using UnityEngine;

[Serializable]
public abstract class WeaponModifier
{
    [HideInInspector]public string name;

    [Range(0f,1f)]
    [SerializeField] float probability;
    [SerializeField] public Rarity rarity;

    protected WeaponModifier() { }


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




