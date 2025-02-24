using System.Collections.Generic;
using System;

[Serializable]
public class Weapon_Attack_Data_AOE : Weapon_Attack_Data_Base
{
    public float Range;


    // Constructor for Weapon_Attack_Melee
    public Weapon_Attack_Data_AOE(AttackType type = AttackType.AOE)
    {
        attackType = type;
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
