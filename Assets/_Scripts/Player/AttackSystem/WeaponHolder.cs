using System.Collections;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    public bool QueueDebugMessages;
    private PlayerInput playerInput;

    int ComboCounter;
    
    [HideInInspector]public Weapon_Attack_Data_Base CurrentAttackData;

    
    

    //Define statemachine states
    public enum AttackState
    {
        Attacking,
        Ready,
        Combo,
        Cooldown,
        Charging
    }

    //State variable
    [SerializeField]public AttackState State = AttackState.Ready;

    [SerializeField] public WeaponInstance instance;
    [SerializeField] public WeaponDataSO data;
    [SerializeField] public LayerMask DamagableLayer;
    public Animator Weapon_anim;
    public Camera cam;
    public float ChargeAmount;



    bool IsAttack2;
    bool IsAttack3;
    //Called to set the weapon data once weapon swapping is implemented
    public void SetWeaponInstance(WeaponInstance instance, bool A2 =false, bool A3 = false)
    {
        //if (!enabled && instance != null) enabled = true; 
        this.instance = instance;
        this.data = instance.data;
        IsAttack2 = A2;
        IsAttack3 = A3;
        HandleWeaponSwapping();
    }

    public Transform handPos;
    public Transform secondaryPos;
    public GameObject WeaponModel;
    public GameObject SecondaryModel;

    public void HandleWeaponSwapping()
    {
        Destroy(WeaponModel);
        WeaponModel = Instantiate(data.model, handPos);
        WeaponModel.layer = 7;

        Destroy(SecondaryModel);
        if (data.secondaryModel != null) SecondaryModel = Instantiate(data.secondaryModel, secondaryPos);


        ChargeAmount = 1;
        Weapon_anim.runtimeAnimatorController = data.Anim_controller;
    }

    //Initialization steps
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
        State = AttackState.Ready;
        //handPos = GameObject.Find("HandModel").transform;
        ComboCounter = 0;

        if(data == null)
        {
            enabled = false;
        }

    }


    private void Update()
    {
        
    }

    //Calling Input from Queue
    public void DoQueuedInput(int i, InputAction.CallbackContext ctx)
    {
        if(!enabled) return;
        data.Weapon_Attacks[i].weaponInputLogic.QueuedInput(i, this, ctx);
    }


    //Check inputs for different attacks 1, 2, 3 (Handled with 3 functions since this is how the input system works best [I think])
    public void Attack1Input(InputAction.CallbackContext ctx)
    {
        if (!enabled) return;
        data.Weapon_Attacks[0].weaponInputLogic._Input(0, this, ctx);
    }

    public void Attack2Input(InputAction.CallbackContext ctx)
    {
        if (!enabled || !IsAttack2) return;
        data.Weapon_Attacks[1].weaponInputLogic._Input(1, this, ctx);
    }

    public void Attack3Input(InputAction.CallbackContext ctx)
    {
        if (!enabled || !IsAttack3) return;
        data.Weapon_Attacks[2].weaponInputLogic._Input(2, this, ctx);
    }
    
    //Called from weapon Input logic when conditions are met
    public void EnterAttack(int i)
    {
        if (!this.enabled) return;
        //Set State and variables
        State = AttackState.Attacking;
        CurrentAttackData = data.Weapon_Attacks[i];
        
        

        //Handle Animation stuff
        Weapon_anim.SetBool("Attacking", true);

        Weapon_anim.SetInteger("AttackType", i);

        Weapon_anim.SetInteger("ComboInt", ComboCounter);

        //Combocounter allows different animations for the same attack
        ComboCounter++;
        if (ComboCounter >= CurrentAttackData.ComboLength) ComboCounter = 0;

        //Testing
        //CurrentAttackData.PerformAttack(this);

    }


    public void ExitAttack()
    {
        //Reset basically
        HandleCooldownStuff();
        CurrentAttackData = null;
        
        
        ComboCounter = 0;
        ChargeAmount = 1;
        
        //Handle Animation stuff
        Weapon_anim.SetBool("Attacking", false);
        Weapon_anim.SetBool("Combo", false);
        Weapon_anim.SetBool("Charging", false);
    }

    public void HandleCooldownStuff()
    {
        if (CurrentAttackData.hasCooldown)
        {
            State = AttackState.Cooldown;
            StartCoroutine(Cooldown(CurrentAttackData.cooldown));
        }
        else
        {
            State = AttackState.Ready;
            TryExecuteQueuedAttack();
        }
    }

    //Allows for a cooldown of attacks
    IEnumerator Cooldown(float t)
    {
        yield return new WaitForSeconds(t);
        if(State != AttackState.Charging) {
            State = AttackState.Ready;
        }
        TryExecuteQueuedAttack();
    }


    #region Input Queuing
    //Input Queuing

    private bool queuedAttack = false;
    private int queuedAttackNum;
    private InputAction.CallbackContext queuedContext;
    private float queueTime;
    private float queueExpirationTime;

    public void QueueAttack(int attackNum, InputAction.CallbackContext ctx, float expirationTime)
    {
        if(QueueDebugMessages) Debug.Log("Queued Attack");
        queuedAttack = true;
        queuedAttackNum = attackNum;
        queuedContext = ctx;
        queueTime = Time.time;
        queueExpirationTime = expirationTime;
    }

    public void TryExecuteQueuedAttack()
    {
        if (!queuedAttack) return;

        // Check if the queued attack has expired
        if (Time.time - queueTime > queueExpirationTime)
        {
            if (QueueDebugMessages) Debug.Log("Queue Expired");
            queuedAttack = false;
            return;
        }

        if (State == AttackState.Ready || (State == AttackState.Combo && data.Weapon_Attacks[queuedAttackNum].ComboLength>1)) // Player can attack now
        {
            if (QueueDebugMessages) Debug.Log("Executing Queued Attack");

            // Do attack BEFORE resetting the queue
            DoQueuedInput(queuedAttackNum, queuedContext);

            // reset queue after attack
            queuedAttack = false;
        }
    }

    #endregion


    //Spawns objects could be projectiles or particles 
    public GameObject SpawnObject(GameObject go, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        GameObject inst = Instantiate(go, pos, rot, parent);
        return inst;
    }

    //All functions below are called by animation event handler
    public void AttackPerformed()
    {
        CurrentAttackData.PerformAttack(this);
    }

    //Allows the player to chain attacks
    public void OpenComboWindow()
    {
        State = AttackState.Combo;
        TryExecuteQueuedAttack();
        Weapon_anim.SetBool("Combo", true);
    }

    //Closes the opportunity to chain attacks
    public void CloseComboWindow()
    {
        State = AttackState.Attacking;
        Weapon_anim.SetBool("Combo", false);
    }

    //the 
    public void StartAttackCharging(int Attack)
    {
        State = AttackState.Charging;
        Weapon_anim.SetBool("Charging", true);
        Weapon_anim.SetInteger("AttackType", Attack);

    }


    
    
}



