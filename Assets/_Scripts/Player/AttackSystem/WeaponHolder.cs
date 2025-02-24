using System.Collections;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{

    int ComboCounter;
    Weapon_Attack_Data_Base CurrentAttackData;
    [SerializeField]public LayerMask DamagableLayer;
    private enum AttackState
    {
        Attacking,
        Ready,
        Combo,
        Cooldown
    }

    AttackState State;

    [SerializeField] private WeaponDataSO data;

    public void SetWeaponData(WeaponDataSO data)
    {
        this.data = data;
    }


    private void Start()
    {
        State = AttackState.Ready;
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            EnterAttack(data.Weapon_Attacks[0]);
        }
    }


    

    public void EnterAttack(Weapon_Attack_Data_Base attackData)
    {
        Debug.Log("Attacking!");
        State = AttackState.Attacking;
        CurrentAttackData = attackData;
        ComboCounter++;
        if(ComboCounter == attackData.ComboLength) ComboCounter = 0;


        //Testing for now
        CurrentAttackData.PerformAttack(this);

    }


    public void ExitAttack()
    {
        CurrentAttackData = null;
        State = AttackState.Cooldown;
    }


    IEnumerator Cooldown(float t)
    {
        yield return new WaitForSeconds(t);
        State = AttackState.Ready;
    }


    
}
