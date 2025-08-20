using UnityEngine;

public class Weapon_Input : ScriptableObject
{
    [Tooltip("How long a buffered press stays valid.")]
    public float queueExpirationTime = 0.5f;

    public virtual void OnPress(int attackIndex, WeaponHolder wh, bool alt)
    {
        Debug.Log("Pressed");
        wh.EnterAttack(attackIndex, false);
    }

    public virtual void OnRelease(int attackIndex, WeaponHolder wh, bool alt)
    {
        
        Debug.Log("Released");
    }

    public virtual void OnHoldTick(int attackIndex, WeaponHolder wh) { }

   
    public virtual bool StartsChargingOnPress => false;

    public bool CheckStateOK(WeaponHolder wh)
        => wh.State == WeaponHolder.AttackState.Ready || wh.State == WeaponHolder.AttackState.Combo;
}

