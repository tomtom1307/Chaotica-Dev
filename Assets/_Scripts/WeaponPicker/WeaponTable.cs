using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponTable", menuName = "WeaponTable")]
public class WeaponTable : ScriptableObject
{
    public List<PossibleWeapon> possibleWeapons;
}

[Serializable]
public struct PossibleWeapon
{
    public WeaponDataSO weaponData;
    public int weight;
}
