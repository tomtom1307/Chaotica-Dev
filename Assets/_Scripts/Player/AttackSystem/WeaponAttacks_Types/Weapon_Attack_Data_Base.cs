using Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[Serializable]
public class Weapon_Attack_Data_Base
{
    public string Name;
    [TextArea(4, 1)]
    public string Description;
    public Weapon_Input weaponInputLogic;
    [ReadOnlyy, SerializeField]public AttackType attackType;
    public DamageType damageType = DamageType.Standard;

    [Header("Stats")]
    public float damage = 100;
    public float StaggerDamage = 5;
    [Min(0)] public float ChargeTime;
    public float KnockBackForce = 100;

    [Header("Juice")]
    public float AttackWeight = 1;
    public float HitFOV = 10;
    public List<Vector3> Forces;
    public GameObject VFX;
    

    [Header("Combo")]
    public float ComboLength = 1;
    public float FinalHitMultiplier = 1;
    
    
    
    public float MoveSpeedMult = 1;
    public bool AllowAgility = false;
    
    public bool hasCooldown = true;
    public float cooldown = 0.1f;

    [Header("Ammo Cost (weapon-wide pool)")]
    [Min(0)] public int ammoCost = 0; // 0 = fre


    [Header("Interrupt")]
    public bool allowAttackInterupt = false;

    

    //TODO: Store attack chargeup time on here instead of on weapon input (Custom Editor)
    public virtual void EnterAttack(WeaponHolder W) 
    {
        
    }
    public virtual void PerformAttack(WeaponHolder W) {
        FOVFXController.instance.PlayImpulse(HitFOV, 0.1f, 0.2f);
    }
    public virtual void ExitAttack(WeaponHolder W) { }

    public (float, bool) DamageVal(WeaponHolder W) {
        float mult = 1;
        if(W.ComboCounter == 0 || W.alt)
        {
            mult = FinalHitMultiplier;
        }

        //CheckIfCrit
        bool isCrit = UnityEngine.Random.value <= PlayerStats.instance.GetStat(StatType.CritChance);
        if (isCrit)
        {
            GameManager.instance.TriggerHitStop(0.05f);
            CamShake.instance.StartShake(CamShake.instance.onHit);
            mult *= PlayerStats.instance.GetStat(StatType.CritMultiplier);
        }

        float DamageValue = mult*W.DamageBonus(damageType) * W.ChargeAmount * 0.01f * damage * W.data.WeaponDamage;
        return (DamageValue, isCrit);
    }

    public void DealDamage(WeaponHolder W, Damagable damagable, float Multiplier  =  1) 
    {
        float Damage = 0;
        bool isCrit = false;
        (Damage, isCrit) = DamageVal(W);
        Damage *= Multiplier;

        damagable.TakeDamage(Damage,crit: isCrit);
        W.instance.TryTriggerProcs(W, damagable, Multiplier);
    }

    public void DealDamage(WeaponHolder W, Damagable damagable, RaycastHit hit, float Multiplier = 1)
    {
        float Damage = 0;
        bool isCrit = false;
        (Damage, isCrit) = DamageVal(W);
        Damage *= Multiplier;

        damagable.TakeDamage(Damage, hit.point, hit.normal, isCrit);

        W.instance.TryTriggerProcs(W, damagable, damage, hit);


    }


    public void ApplyForceToPlayer(WeaponHolder W,int i)
    {
        Vector3 Force = Forces[i].x * W.playerMovement.orientation.right +  Forces[i].y * Vector3.up +Forces[i].z*W.playerMovement.orientation.forward ;
        W.rb.AddForce(Force,ForceMode.VelocityChange);
    }

    public void ApplyKnockback(Collider c, Vector3 v)
    {
        var knockbackable = GetKnockbackAble(c);
        knockbackable?.GetKnockedBack(KnockBackForce * v);
        
    }

    public void ApplyKnockback(Collider c, Vector3 v, Vector3 point)
    {
        var knockbackable = GetKnockbackAble(c);
        knockbackable?.GetKnockedBack(KnockBackForce*v, point);
    }

    public static void ApplyKnockback_Mag(Collider c, Vector3 v, Vector3 point)
    {
        var knockbackable = GetKnockbackAble(c);
        knockbackable?.GetKnockedBack(v, point);
    }

    public static IKnockbackable GetKnockbackAble(Collider c)
    {
        IKnockbackable knockbackable;
        if (c.TryGetComponent<IKnockbackable>(out knockbackable))
        {
            return knockbackable;
        }
        knockbackable = c.GetComponentInParent<IKnockbackable>();
        if(knockbackable != null)
        {
            return knockbackable;
        }
        knockbackable = c.GetComponentInChildren<IKnockbackable>();
        if (knockbackable != null)
        {
            return knockbackable;
        }
        else return null;
    }


}






public enum AttackType
{
    Melee,
    Projectile,
    Raycast,
    AOE,
    BlockParry
}

public enum DamageType
{
    Standard,
    Umbraveil,
    Scarforge,
    Verdancy,
    Aetherflow
}


