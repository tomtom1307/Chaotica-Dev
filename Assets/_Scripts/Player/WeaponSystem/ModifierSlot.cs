using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[Serializable]
public class ModifierSlot
{
    

    public WeaponDataSO.Rarity RarityTier;
    public WeaponModifier Equipped;


    public bool CanEquip(WeaponModifier mod)
    {
        return mod.rarity <= RarityTier && Equipped == null;
    }

    public bool Equip(WeaponModifier mod)
    {
        if(!CanEquip(mod)) return false;
        Equipped = mod;
        return true;
    }
}
