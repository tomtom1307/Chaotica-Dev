using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon_Input : ScriptableObject
{
    public float queueExpirationTime = 0.5f; // Input expires after 0.5 seconds
    public virtual void _Input(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx)
    {
    }

    public virtual void QueuedInput(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx, bool alt = false)
    {
        WH.EnterAttack(AttackNum, alt);
    }


    public bool CheckState(int attackNum,WeaponHolder WH) { 
        return (WH.State == WeaponHolder.AttackState.Ready || (WH.State == WeaponHolder.AttackState.Combo));
    }
}
