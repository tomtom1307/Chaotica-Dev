using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Weapon_Attack_Data_Base
{
    public Weapon_Input weaponInputLogic;
    public AttackType attackType;
    public DamageType damageType = DamageType.Standard;
    public float damage = 5;
    public float ComboLength = 1;
    public float manaUse = 5;
    public float StaggerDamage = 5;

    public bool hasCooldown = false;
    public float cooldown = 0.1f;
    
    //TODO: Store attack chargeup time on here instead of on weapon input (Custom Editor)
    public virtual void EnterAttack(WeaponHolder W) { }
    public virtual void PerformAttack(WeaponHolder W) { }
    public virtual void ExitAttack(WeaponHolder W) { }

    public float DamageVal(WeaponHolder W) {
        float DamageValue = W.ChargeAmount * 0.01f * damage * W.data.WeaponDamage;
        return DamageValue;
    }

    public void DealDamage(WeaponHolder W, Damagable damagable) {
        damagable.TakeDamage(DamageVal(W));
    }
    
}






public enum AttackType
{
    Melee,
    Projectile,
    Raycast,
    AOE
}

public enum DamageType
{
    Standard,
    Umbraveil,
    Scarforge,
    Verdancy,
    Aetherflow
}
