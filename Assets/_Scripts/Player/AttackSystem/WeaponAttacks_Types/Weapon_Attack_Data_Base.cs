using Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    [ReadOnlyy, SerializeField]protected AttackType attackType;
    public DamageType damageType = DamageType.Standard;
    public float damage = 100;
    public float ComboLength = 1;
    public float StaggerDamage = 5;
    public float AttackWeight = 1;
    public float MoveSpeedMult = 1;
    public bool hasCooldown = true;
    public float cooldown = 0.1f;
    public bool AllowAgility = false;
    public float KnockBackForce = 100;
    public float FinalHitMultiplier = 100;
    public float HitFOV = 120;
    public List<Vector3> Forces;
    public GameObject VFX;




    //TODO: Store attack chargeup time on here instead of on weapon input (Custom Editor)
    public virtual void EnterAttack(WeaponHolder W) 
    {
        
    }
    public virtual void PerformAttack(WeaponHolder W) {
        FOVFXController.instance.PunchFOV(HitFOV, 0.2f, 0.1f, 0.2f);
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

    public void DealDamage(WeaponHolder W, Damagable damagable) 
    {
        float Damage = 0;
        bool isCrit = false;
        (Damage, isCrit) = DamageVal(W);
       
        TryEnablingPhysics(damagable);

        damagable.TakeDamage(Damage,crit: isCrit);
        W.instance.TryTriggerProcs(W, damagable, damage);
    }

    public void DealDamage(WeaponHolder W, Damagable damagable, RaycastHit hit)
    {
        float Damage = 0;
        bool isCrit = false;
        (Damage, isCrit) = DamageVal(W);
        
        TryEnablingPhysics(damagable);

        damagable.TakeDamage(Damage, hit.point, hit.normal, isCrit);

        W.instance.TryTriggerProcs(W, damagable, damage, hit);
    }

    public static void TryEnablingPhysics(Damagable damagable)
    {
        DamagableEnemy damagableEnemy = damagable as DamagableEnemy;

        if (damagableEnemy != null)
        {
            damagableEnemy.brain.TogglePhysics(true);
        }
    }

    public void ApplyForceToPlayer(WeaponHolder W,int i)
    {
        
        Vector3 Force = Forces[i].x * W.playerMovement.orientation.right +  Forces[i].y * Vector3.up +Forces[i].z*W.playerMovement.orientation.forward ;
        W.rb.AddForce(Force,ForceMode.VelocityChange);
    }

    public void ApplyKnockback(Collider c, Vector3 v)
    {
        Rigidbody hitrb;
        if (c.gameObject.TryGetComponent<Rigidbody>(out hitrb))
        {
            hitrb.AddForce(KnockBackForce * v.normalized);
        } else
        {
            hitrb = c.gameObject.GetComponentInParent<Rigidbody>();

            if (hitrb != null)
            {
                hitrb.AddForce(KnockBackForce * v.normalized);
            }
        }
    }

    public void ApplyKnockback(Collider c, Vector3 v, Vector3 point)
    {
        Rigidbody hitrb;
        if (c.gameObject.TryGetComponent<Rigidbody>(out hitrb))
        {
            hitrb.AddForceAtPosition(KnockBackForce * v.normalized, point);
        } else
        {
            hitrb = c.gameObject.GetComponentInParent<Rigidbody>();

            if (hitrb != null)
            {
                hitrb.AddForce(KnockBackForce * v.normalized);
            }
        }
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


