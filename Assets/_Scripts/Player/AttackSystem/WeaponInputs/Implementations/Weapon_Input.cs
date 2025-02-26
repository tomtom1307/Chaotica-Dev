using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon_Input : ScriptableObject
{
    public float queueExpirationTime = 0.5f; // Input expires after 0.5 seconds
    public virtual void _Input(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx)
    {
    }

    public virtual void QueuedInput(int AttackNum, WeaponHolder WH, InputAction.CallbackContext ctx)
    {
        WH.EnterAttack(AttackNum);
    }


    public bool CheckState(int attackNum,WeaponHolder WH) { 
        return (WH.State == WeaponHolder.AttackState.Ready || (WH.State == WeaponHolder.AttackState.Combo && WH.data.Weapon_Attacks[attackNum].ComboLength > 1));
    }
}
