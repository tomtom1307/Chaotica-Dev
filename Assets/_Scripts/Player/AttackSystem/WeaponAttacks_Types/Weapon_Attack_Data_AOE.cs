using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

[Serializable]
public class Weapon_Attack_Data_AOE : Weapon_Attack_Data_Base
{
    PlayerMovement playerMovement;
    
    
    public float attack_range;


    // Constructor for Weapon_Attack_Melee
    public Weapon_Attack_Data_AOE(AttackType type = AttackType.AOE)
    {
        attackType = type;
    }



    public override void PerformAttack(WeaponHolder W)
    {
        base.PerformAttack(W);
        base.PerformAttack(W);
        Debug.Log("did the Thing");
        List<Collider> col = Physics.OverlapSphere(W.transform.position, attack_range, W.DamagableLayer).ToList();
        Debug.Log(col.Count);
        foreach (Collider c in col)
        {
            Debug.Log("Detected a homie " + c.name);
            Vector3 dirVec = c.transform.position - Camera.main.transform.position;
            Damagable D = c.GetComponent<Damagable>();
            D.TakeDamage(damage);
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
