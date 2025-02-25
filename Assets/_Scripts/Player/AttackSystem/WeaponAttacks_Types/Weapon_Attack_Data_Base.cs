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
    

    public virtual void EnterAttack(WeaponHolder wh) { }
    public virtual void PerformAttack(WeaponHolder wh) { }
    public virtual void ExitAttack(WeaponHolder wh) { }



    
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
