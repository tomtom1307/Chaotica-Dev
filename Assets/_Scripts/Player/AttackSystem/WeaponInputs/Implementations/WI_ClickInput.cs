using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "WI_ClickInput", menuName = "WeaponInputs/ClickInput")]
public class ClickInput : Weapon_Input
{
    public override void _Input(int AttackNum,WeaponHolder WH, InputAction.CallbackContext ctx)
    {
        if(ctx.started && CheckState(WH)) //Button Press
        {
            WH.EnterAttack(AttackNum); // Do Attack
        }
        
    }
}
