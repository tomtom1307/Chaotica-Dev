using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "WI_HoldRelease", menuName = "WeaponInputs/Hold and Release")]
public class WI_HoldRelease : Weapon_Input
{

    bool performedHold;
    public override void _Input(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx)
    {
        
        if (ctx.started)
        {
            WH.Weapon_anim.SetInteger("ComboInt", 0);
            if (CheckState(AttackNum, WH))
            {
                
                WH.ComboCounter = 0;
                WH.EnterAttack(AttackNum, true);
            }
            else
            {
                WH.QueueAttack(AttackNum, ctx, queueExpirationTime, alt: true);
            }
        }

        //Button UP
        if (ctx.ReadValue<float>() == 0)
        {
            Debug.Log("Button Up");
            WH.Weapon_anim.SetInteger("ComboInt", 1);
        }


    }

}
