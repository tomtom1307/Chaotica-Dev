using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "WI_HoldRelease", menuName = "WeaponInputs/Hold and Release")]
public class WI_HoldRelease : Weapon_Input
{

    bool performedHold;
    public override void _Input(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx)
    {
        if (ctx.interaction is HoldInteraction)
        {
            if (ctx.started)
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
                WH.Weapon_anim.SetBool("Attacking", false);
            }
        }
    }
}
