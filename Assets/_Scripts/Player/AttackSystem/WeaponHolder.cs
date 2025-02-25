using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{

    private PlayerInput playerInput;

    int ComboCounter;
    public Camera cam;
    Weapon_Attack_Data_Base CurrentAttackData;
    [SerializeField]public LayerMask DamagableLayer;
    public Animator Weapon_anim;
    public enum AttackState
    {
        Attacking,
        Ready,
        Combo,
        Cooldown,
        Charging
    }

    [SerializeField]public AttackState State = AttackState.Ready;

    [SerializeField] private WeaponDataSO data;

    public float ChargeAmount;


    public void SetWeaponData(WeaponDataSO data)
    {
        this.data = data;
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
        State = AttackState.Ready;

        ComboCounter = 0;
    }


    private void Update()
    {
        
    }

    public void Attack1Input(InputAction.CallbackContext ctx)
    {
        data.Weapon_Attacks[0].weaponInputLogic._Input(0, this, ctx);
    }

    public void Attack2Input(InputAction.CallbackContext ctx)
    {
        data.Weapon_Attacks[1].weaponInputLogic._Input(1, this, ctx);
    }

    public void Attack3Input(InputAction.CallbackContext ctx)
    {
        data.Weapon_Attacks[2].weaponInputLogic._Input(2, this, ctx);
    }
    

    public void EnterAttack(int i)
    {
        Debug.Log("Enter Attack!");

        State = AttackState.Attacking;
        CurrentAttackData = data.Weapon_Attacks[i];
        
        

        //Handle Animation stuff
        Weapon_anim.SetBool("Attacking", true);

        Weapon_anim.SetInteger("AttackType", i);

        Weapon_anim.SetInteger("ComboInt", ComboCounter);


        ComboCounter++;
        if (ComboCounter >= CurrentAttackData.ComboLength) ComboCounter = 0;

        //Testing
        //CurrentAttackData.PerformAttack(this);

    }


    public void ExitAttack()
    {
        //Reset basically
        Debug.Log("AttackExit");
        StartCoroutine(Cooldown(CurrentAttackData.cooldown));
        CurrentAttackData = null;
        
        State = AttackState.Cooldown;
        ComboCounter = 0;
        ChargeAmount = 0;

        //Handle Animation stuff
        Weapon_anim.SetBool("Attacking", false);
        Weapon_anim.SetBool("Combo", false);
        Weapon_anim.SetBool("Charging", false);
    }



    public void AttackPerformed()
    {
        CurrentAttackData.PerformAttack(this);
    }

    public void OpenComboWindow()
    {
        State = AttackState.Combo;
        Weapon_anim.SetBool("Combo", true);
    }

    public void CloseComboWindow()
    {
        State = AttackState.Attacking;
        Weapon_anim.SetBool("Combo", false);
    }

    public void StartAttackCharging(int Attack)
    {
        State = AttackState.Charging;
        Weapon_anim.SetBool("Charging", true);
        Weapon_anim.SetInteger("AttackType", Attack);

    }


    IEnumerator Cooldown(float t)
    {
        yield return new WaitForSeconds(t);
        State = AttackState.Ready;
    }

    public GameObject SpawnObject(GameObject go, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        GameObject inst = Instantiate(go, pos, rot, parent);
        return inst;
    }
    
}



