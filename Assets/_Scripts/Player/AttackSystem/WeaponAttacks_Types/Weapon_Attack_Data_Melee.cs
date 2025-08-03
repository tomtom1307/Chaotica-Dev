using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

[Serializable]
public class Weapon_Attack_Data_Melee : Weapon_Attack_Data_Base
{
    public float attack_range = 3;
    public float MaxViewAngle = 45;

    // Constructor for Weapon_Attack_Melee
    public Weapon_Attack_Data_Melee(AttackType attackType = AttackType.Melee)
    {
        
    }

    public float secondaryDamageMultiplier = 100;

    public override void PerformAttack(WeaponHolder W)
    {
        base.PerformAttack(W);

        List<Collider> col = Physics.OverlapSphere(W.transform.position, attack_range, W.DamagableLayer).ToList();

        Dictionary<Collider, float> targetAngles = new Dictionary<Collider, float>();
        List<Collider> validTargets = new List<Collider>();

        foreach (Collider c in col)
        {
            if (c.gameObject.tag == "Head") continue;

            Vector3 dirVec = c.transform.position - Camera.main.transform.position;
            float angle = Vector3.Angle(dirVec, Camera.main.transform.forward);

            if (angle < MaxViewAngle)
            {
                targetAngles[c] = angle;
                validTargets.Add(c);
            }
        }

        if (validTargets.Count == 0) return;

        // Sort by smallest angle
        Collider primaryTarget = validTargets.OrderBy(c => targetAngles[c]).First();

        // PRIMARY TARGET DAMAGE
        RaycastHit hit;
        Vector3 dirToPrimary = primaryTarget.transform.position - Camera.main.transform.position;

        Damagable D_primary = Damagable.CheckForDamagable(primaryTarget.gameObject);

        if (Physics.Raycast(Camera.main.transform.position, dirToPrimary.normalized, out hit, attack_range, W.DamagableLayer))
        {
            DealDamage(W, D_primary, hit);
            ApplyKnockback(primaryTarget, dirToPrimary, hit.point);
        }
        else
        {
            DealDamage(W, D_primary);
            ApplyKnockback(primaryTarget, dirToPrimary);
        }

        // SECONDARY TARGETS
        foreach (Collider c in validTargets)
        {
            if (c == primaryTarget) continue;

            Damagable D_secondary = Damagable.CheckForDamagable(c.gameObject);
            Vector3 dirToSecondary = c.transform.position - Camera.main.transform.position;

            if (Physics.Raycast(Camera.main.transform.position, dirToSecondary.normalized, out hit, attack_range, W.DamagableLayer))
            {
                DealDamage(W, D_secondary, hit, secondaryDamageMultiplier);
                ApplyKnockback(c, dirToSecondary, hit.point);
            }
            else
            {
                DealDamage(W, D_secondary, secondaryDamageMultiplier);
                ApplyKnockback(c, dirToSecondary);
            }
        }
    }


    public override void EnterAttack(WeaponHolder W)
    {
        base.EnterAttack(W);
    }

    public override void ExitAttack(WeaponHolder W)
    {
        base.ExitAttack(W);
    }

}
