using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "WI_ChargeHoldRelease", menuName = "WeaponInputs/ChargeHoldRelease")]
public class ChargeHoldRelease: Weapon_Input
{
    public float ChargeTime;
    private float chargeStartTime; // Store when charging starts
    private bool isCharging; // Track if we are charging


    public override void QueuedInput(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx)
    {
        Debug.Log("Did Queued Input");
        HoldLogic(AttackNum, WH, ctx);
    }

    public override void _Input(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx)
    {
        if(WH.State == WeaponHolder.AttackState.Charging)
        {
            HoldLogic(AttackNum, WH, ctx);
        }

        if (!CheckState(AttackNum ,WH))
        {
            
            if (ctx.performed)
            {
                Debug.Log("Queued Hold");
                WH.QueueAttack(AttackNum, ctx, queueExpirationTime);
            }
            return;
        }
        HoldLogic(AttackNum, WH, ctx);


    }

    public void HoldLogic(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !isCharging) // Button pressed down
        {
            Debug.Log("Performed Hold" + "CurrentState:" + WH.State);
            WH.StartAttackCharging(AttackNum);
            chargeStartTime = Time.time;
            isCharging = true;
            HUDController.instance.StartFill(ChargeTime);
        }
        else if (ctx.canceled && isCharging) // Button released
        {
            HUDController.instance.StopFill();
            Debug.Log("Released Hold");
            float totalCharge = Time.time - chargeStartTime;
            isCharging = false;

            if (totalCharge >= ChargeTime)
            {
                WH.ChargeAmount = 1;
            }
            else
            {
                WH.ChargeAmount = totalCharge / ChargeTime;
            }

            WH.EnterAttack(AttackNum);
        }
    }

}
