using UnityEngine;

public class Weapon_Attack_Data_BlockParry : Weapon_Attack_Data_Melee
{
    public float DamageReduction;
    public Weapon_Attack_Data_BlockParry()
    {
        attackType = AttackType.BlockParry;
    }



    public override void PerformAttack(WeaponHolder W)
    {
        W.TriggerBlock(true);
        Debug.Log("Did the thing");
    }

    public void Parry(WeaponHolder W)
    {
        base.PerformAttack(W);
    }

    public override void EnterAttack(WeaponHolder W)
    {
        base.EnterAttack(W);
        Debug.Log("blocknparry");
    }


    public override void ExitAttack(WeaponHolder W)
    {
        base.ExitAttack(W);
        W.TriggerBlock(false);
    }
}
