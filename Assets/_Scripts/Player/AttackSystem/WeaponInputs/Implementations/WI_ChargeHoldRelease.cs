using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "WI_ChargeHoldRelease", menuName = "WeaponInputs/ChargeHoldRelease")]
public class ChargeHoldRelease: Weapon_Input
{
    public float ChargeTime;
    private float chargeStartTime; // Store when charging starts
    private bool isCharging; // Track if we are charging
    public override void _Input(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx)
    {
        if(WH.State != WeaponHolder.AttackState.Charging && !isCharging)
        {
            if (!CheckState(WH)) return;
        }
        

        if (ctx.started) // Button pressed down
        {
            WH.StartAttackCharging(AttackNum); //Tell animator to go to charge state
            chargeStartTime = Time.time; // Start tracking charge time
            isCharging = true;
        }
        else if (ctx.canceled && isCharging) // Button released
        {
            float totalCharge = Time.time - chargeStartTime; // Calculate charge duration
            isCharging = false;

            if (totalCharge >= ChargeTime)
            {
                // Fully charged attack
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
