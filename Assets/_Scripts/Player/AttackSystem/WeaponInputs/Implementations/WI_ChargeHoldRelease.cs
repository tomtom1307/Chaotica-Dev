using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "WI_ChargeHoldRelease", menuName = "WeaponInputs/ChargeHoldRelease")]
public class ChargeHoldRelease: Weapon_Input
{
    private float _pressTime;
    float minHold = 0.05f;
    public override void OnPress(int attackIndex, WeaponHolder wh, bool alt)
    {
        _pressTime = Time.time;
        wh.StartAttackCharging(attackIndex);
        HUDController.instance.StartFill(wh.CurrentAttackData.ChargeTime);
    }

    public override void OnHoldTick(int attackIndex, WeaponHolder wh)
    {
        base.OnHoldTick(attackIndex, wh);
    }


    public override void OnRelease(int attackIndex, WeaponHolder wh, bool alt)
    {
        HUDController.instance.StopFill();
        // Prefer the attack that started charging; fall back to the data list if needed
        var ad = wh.CurrentAttackData ?? (wh.data != null && attackIndex >= 0 && attackIndex < wh.data.Weapon_Attacks.Count
                   ? wh.data.Weapon_Attacks[attackIndex]
                   : null);

        // Use per-attack values (with safe fallbacks)
        float full = ad != null ? ad.ChargeTime : 0.8f;
        float min = ad != null ? minHold : 0.15f;

        // Avoid divide-by-zero; treat <=0 as instant full charge
        float held = Mathf.Max(0f, Time.time - _pressTime);
        wh.ChargeAmount = (full <= 0f) ? 1f : Mathf.Clamp01(held / full);

        // (Optional) if you want tap behavior when released too early:
        if (held < min) { wh.ExitAttack(); return; }

        wh.EnterAttack(attackIndex, alt); // fires on release
    }

    public override bool StartsChargingOnPress => false;

}


