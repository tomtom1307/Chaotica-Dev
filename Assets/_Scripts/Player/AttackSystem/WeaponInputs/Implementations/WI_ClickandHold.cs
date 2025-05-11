using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "WI_ClickandHold", menuName = "WeaponInputs/Click and Hold")]
public class WI_ClickandHold : Weapon_Input
{

    bool performedHold;
    public override void _Input(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx)
    {
        if (ctx.interaction is HoldInteraction)
        {
            if (ctx.performed)
            {
                if (CheckState(AttackNum, WH))
                {
                    WH.EnterAttack(AttackNum, true);
                    Debug.Log("IsHold");
                }
                else
                {
                    WH.QueueAttack(AttackNum, ctx, queueExpirationTime, alt: true);
                }
            }
            if (WH.Weapon_anim.GetBool("Alt") && ctx.canceled && !CheckState(AttackNum, WH))
            {
                WH.ExitAttack();
            }
        }
        if (ctx.interaction is TapInteraction)
        {
            if (ctx.performed)
            {
                performedHold = false;
                if (CheckState(AttackNum, WH)) 
                {
                    WH.EnterAttack(AttackNum);
                }
            }
        }
        
    }
}
