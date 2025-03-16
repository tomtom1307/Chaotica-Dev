using System;
using UnityEngine;


[Serializable]
public class WeaponInstance
{
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

}
