using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "WI_ClickInput", menuName = "WeaponInputs/ClickInput")]
public class ClickInput : Weapon_Input
{
    public override void _Input(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx)
    {
        if (ctx.started && ctx.interaction is HoldInteraction) // Button Pressed but avoid tap interaction
        {
            if (CheckState(AttackNum, WH)) // If player is ready to attack
            {
                WH.EnterAttack(AttackNum); // Execute attack immediately
            }
            else
            {
                WH.QueueAttack(AttackNum, ctx, queueExpirationTime);
            }
        }
    }
}


