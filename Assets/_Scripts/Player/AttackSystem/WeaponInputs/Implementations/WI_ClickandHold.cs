using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "WI_ClickandHold", menuName = "WeaponInputs/Click and Hold")]
public class WI_ClickandHold : Weapon_Input
{
    private bool holdTriggered;

    public override void _Input(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            holdTriggered = false; // Reset hold flag when button is first pressed
        }

        if (ctx.performed && ctx.interaction is HoldInteraction)
        {
            if (!holdTriggered)
            {
                holdTriggered = true;

                // Trigger hold (alt) attack
                if (CheckState(AttackNum, WH))
                    WH.EnterAttack(AttackNum, alt: true);
                else
                    WH.QueueAttack(AttackNum, ctx, queueExpirationTime, alt: true);
            }
        }

        if (ctx.canceled && !holdTriggered)
        {
            // Trigger tap attack if hold wasn't triggered
            if (CheckState(AttackNum, WH))
                WH.EnterAttack(AttackNum);
            else
                WH.QueueAttack(AttackNum, ctx, queueExpirationTime);
        }
    }
}
