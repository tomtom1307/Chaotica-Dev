using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class WeaponInstance
{
    public List<ProcableEffectBase> ProcableAbilityList = new List<ProcableEffectBase>();


    public WeaponInstance(WeaponDataSO weaponDataSO, int KillThresh1 = 10, int KillThresh2 = 50)
    {
        Level = 1;
        data = weaponDataSO;
        KillCount = 0;
        Threshold1 = KillThresh1;
        Threshold2 = KillThresh2;
    }


    public WeaponDataSO data;
    public int Level;
    public int KillCount;
    public int Threshold1;
    public int Threshold2;

    public void LevelUp()
    {
        Level++;
    }

    public void TryTriggerProcs(WeaponHolder holder, Damagable damagable, float damage, RaycastHit? hit = null)
    {
        if (ProcableAbilityList == null) return;

        foreach (var proc in ProcableAbilityList)
        {
            if (proc.TryProc())
            {
                proc.Trigger(holder, damagable, damage, hit);
            }
        }
    }

}
