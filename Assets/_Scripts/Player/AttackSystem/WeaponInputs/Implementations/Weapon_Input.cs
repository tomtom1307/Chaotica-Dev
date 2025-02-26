using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon_Input : ScriptableObject
{
    public virtual void _Input(int AttackNum , WeaponHolder WH, InputAction.CallbackContext ctx)
    {
    }


    public bool CheckState(WeaponHolder WH) { 
        return (WH.State == WeaponHolder.AttackState.Ready || WH.State == WeaponHolder.AttackState.Combo);
    }
}
