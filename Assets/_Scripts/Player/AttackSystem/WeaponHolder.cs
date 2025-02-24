using System.Collections;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{

    int ComboCounter;
    public Camera cam;
    Weapon_Attack_Data_Base CurrentAttackData;
    [SerializeField]public LayerMask DamagableLayer;
    public Animator Weapon_anim; 
    private enum AttackState
    {
        Attacking,
        Ready,
        Combo,
        Cooldown
    }

    [SerializeField] AttackState State = AttackState.Ready;

    [SerializeField] private WeaponDataSO data;

    public void SetWeaponData(WeaponDataSO data)
    {
        this.data = data;
    }

    private void Start()
    {
        cam = Camera.main;
        State = AttackState.Ready;

        ComboCounter = 0;
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && (State == AttackState.Ready || State == AttackState.Combo))
        {
            EnterAttack(0);
        }
        if(Input.GetMouseButtonDown(1) && (State == AttackState.Ready || State == AttackState.Combo))
        {
            EnterAttack(1);
        }
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
        if (ComboCounter > CurrentAttackData.ComboLength) ComboCounter = 0;

        //Testing
        //CurrentAttackData.PerformAttack(this);

    }


    public void ExitAttack()
    {
        Debug.Log("AttackExit");
        StartCoroutine(Cooldown(CurrentAttackData.cooldown));
        CurrentAttackData = null;
        
        State = AttackState.Cooldown;
        ComboCounter = 0;
        //Handle Animation stuff
        Weapon_anim.SetBool("Attacking", false);
        Weapon_anim.SetBool("Combo", false);

        
        
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
