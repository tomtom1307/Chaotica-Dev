using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHUDAttackManager : MonoBehaviour
{
    public WeaponHUD_Attack attackInfoPrefab;

    List<WeaponHUD_Attack> attacks = new List<WeaponHUD_Attack>();
    public void PopulateAttacks(WeaponInstance weaponInstance)
    {
        Wipe();
        foreach(var Attack in weaponInstance.data.Weapon_Attacks)
        {
            WeaponHUD_Attack attackInfo = Instantiate(attackInfoPrefab, transform);
            attackInfo.PopulateAttackData(Attack);
            attacks.Add(attackInfo);
        }
    }

    public void Wipe()
    {
        foreach (var attack in attacks)
        {
            Destroy(attack.gameObject);
        }
        attacks.Clear();
    }
}
