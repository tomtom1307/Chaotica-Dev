using System;
using System.Data;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.WSA;

[Serializable]
public class ModifierSlot
{
    

    public WeaponDataSO.Rarity RarityTier;
    public WeaponModifier Equipped;
    public string ModifierName;


    public bool CanEquip(WeaponModifier mod)
    {
        return mod.rarity <= RarityTier && Equipped == null;
    }

    public bool Equip(WeaponModifier mod)
    {
        if(!CanEquip(mod)) return false;
        Equipped = mod;
        ModifierName = Equipped.name;
        return true;
    }

    public void TryProc(WeaponHolder holder, Damagable damagable, float damage, RaycastHit? hit = null)
    {
        if (Equipped == null) return;
        if (Equipped.TryProc())
        {
            Equipped.Trigger(holder, damagable, damage, hit);
        }
    }

}
