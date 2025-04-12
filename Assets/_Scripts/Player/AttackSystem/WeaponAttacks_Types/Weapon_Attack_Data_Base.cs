using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class Weapon_Attack_Data_Base
{
    public string Name;
    [TextArea(4, 1)]
    public string Description;
    public Weapon_Input weaponInputLogic;
    [ReadOnlyy, SerializeField]protected AttackType attackType;
    public DamageType damageType = DamageType.Standard;
    public float damage = 5;
    public float ComboLength = 1;
    public float manaUse = 5;
    public float StaggerDamage = 5;
    public float AttackWeight = 1;
    public float MoveSpeedMult = 1;
    public bool hasCooldown = false;
    public float cooldown = 0.1f;

    public List<Vector3> Forces;

    //TODO: Store attack chargeup time on here instead of on weapon input (Custom Editor)
    public virtual void EnterAttack(WeaponHolder W) { }
    public virtual void PerformAttack(WeaponHolder W) { }
    public virtual void ExitAttack(WeaponHolder W) { }

    public float DamageVal(WeaponHolder W) {
        float DamageValue = W.DamageBonus(damageType) * W.ChargeAmount * 0.01f * damage * W.data.WeaponDamage;
        return DamageValue;
    }

    public void DealDamage(WeaponHolder W, Damagable damagable) {
        damagable.TakeDamage(DamageVal(W));
    }

    public void ApplyForce(WeaponHolder W,int i)
    {
        Vector3 Force = Forces[i].x * W.playerMovement.orientation.right +  Forces[i].y * Vector3.up + Forces[i].z*W.playerMovement.orientation.forward ;
        W.rb.AddForce(Force,ForceMode.VelocityChange);
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

