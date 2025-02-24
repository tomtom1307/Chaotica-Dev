using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Weapon_Attack_Data_Projectile : Weapon_Attack_Data_Base
{
    public int Projectile_Amount = 1;
    public float spread = 5;
    public float speed = 15;
    public GameObject projectile;

    // Constructor for Weapon_Attack_Melee
    public Weapon_Attack_Data_Projectile()
    {
    }

    

    public override void PerformAttack(WeaponHolder W)
    {
        base.PerformAttack(W);
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
