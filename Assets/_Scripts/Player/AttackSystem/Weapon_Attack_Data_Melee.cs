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


    public override void PerformAttack(WeaponHolder W)
    {
        base.PerformAttack(W);
        Debug.Log("did the Thing");
        List<Collider> col = Physics.OverlapSphere(W.transform.position, attack_range, W.DamagableLayer).ToList();
        Debug.Log(col.Count);
        foreach (Collider c in col)
        {
            Debug.Log("Detected a homie "+c.name);
            Vector3 dirVec = c.transform.position  - W.transform.position;
            if(Vector3.Angle(dirVec, Camera.main.transform.forward) < MaxViewAngle)
            {
                Damagable D = c.GetComponent<Damagable>();
                D.TakeDamage(damage);
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
