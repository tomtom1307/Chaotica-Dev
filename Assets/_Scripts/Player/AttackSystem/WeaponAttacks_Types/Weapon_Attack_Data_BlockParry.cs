using UnityEngine;

public class Weapon_Attack_Data_BlockParry : Weapon_Attack_Data_Base
{
    public float DamageReduction;
    public Weapon_Attack_Data_BlockParry()
    {
        attackType = AttackType.BlockParry;
    }



    public override void PerformAttack(WeaponHolder W)
    {
        base.PerformAttack(W);
        W.TriggerBlock(true);
        Debug.Log("Did the thing");
        
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
