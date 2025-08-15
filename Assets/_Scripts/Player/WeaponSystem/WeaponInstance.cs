using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class WeaponInstance
{
    //public List<WeaponModifier> ProcableAbilityList = new List<WeaponModifier>();

    public WeaponInstance(WeaponDataSO weaponDataSO, int KillThresh1 = 10, int KillThresh2 = 50, Rarity rarity = Rarity.Random)
    {
        Level = 1;
        data = weaponDataSO;
        weaponRarity = DetermineRarity(rarity);
        Initialize_Modifier_Slots();

        KillCount = 0;
        Threshold1 = KillThresh1;
        Threshold2 = KillThresh2;
    }


    public WeaponDataSO data;
    public int Level;
    public int KillCount;
    
    // To Unlock Weapon Attacks (Might Remove TBH)
    [HideInInspector] public int Threshold1;
    [HideInInspector] public int Threshold2;

    public List<ModifierSlot> ModifierSlots = new List<ModifierSlot>();
    public Rarity weaponRarity;

    public void LevelUp()
    {
        Level++;
    }

    public void Initialize_Modifier_Slots()
    {
        int maxTier = (int)weaponRarity;

        for(int tier = 0; tier <= maxTier; tier++)
        {
            ModifierSlots.Add(new ModifierSlot
            {
                RarityTier = (Rarity.Mythic)
                
            });
            
        }
        foreach (var Mod in data.defaultModifiers)
        {
            ModifierSlot modSlot = new ModifierSlot
            {
                RarityTier = (Rarity.Mythic)

            };

            modSlot.Equip(Mod);

            ModifierSlots.Add(modSlot);
        }


    }

    public Rarity DetermineRarity(Rarity rarity)
    {
        if(rarity == Rarity.Random)
        {
            return RollRarity(data);
        }
        else
        {
            return rarity;
        }
    }

    public Rarity RollRarity(WeaponDataSO data)
    {
        return data.rarityDistribution.RollRarity();
    }

    public void TryTriggerProcs(WeaponHolder holder, Damagable damagable, float damage, RaycastHit? hit = null)
    {
        if (ModifierSlots == null) return;

        foreach (var Slot in ModifierSlots)
        {
            Slot.TryProc(holder,damagable, damage, hit);
        }
    }



}
