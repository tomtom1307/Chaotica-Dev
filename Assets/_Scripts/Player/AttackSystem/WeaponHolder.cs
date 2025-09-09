using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CombatInput))]
public class WeaponHolder : MonoBehaviour
{
    // ================================
    //             CONFIG
    // ================================
    public bool QueueDebugMessages;
    public static WeaponHolder Singleton;

    [Header("Refs")]
    public Animator Weapon_anim;
    public Camera cam;
    public PlayerMovement playerMovement;
    public Transform RhandPos;
    public Transform LhandPos;
    public Transform secondaryPos;

    [Header("Weapon Data")]
    [SerializeField] public WeaponInstance instance = null;
    [SerializeField] public WeaponDataSO data;
    [SerializeField] public Secondary_Weapon_Base SecondaryWeaponData;
    [SerializeField] public LayerMask DamagableLayer;

    [Header("Runtime")]
    public int ComboCounter;
    public float ChargeAmount = 1f;
    [HideInInspector] public Weapon_Attack_Data_Base CurrentAttackData;

    // Unlock flags
    private bool _isAttack2Unlocked;
    private bool _isAttack3Unlocked;

    // Models
    public GameObject WeaponModel;
    public GameObject SecondaryModel;

    // IK
    private HandIKHandler IK_Handler;

    // Input
    private PlayerInput _playerInput;

    // Physics
    [HideInInspector] public Rigidbody rb;

    // Status effects
    private readonly List<statusEffectBase> _activeEffects = new();

    // State
    public enum AttackState { Attacking, Ready, Combo, Cooldown, Charging }
    [SerializeField] public AttackState State = AttackState.Ready;

    // Attack queue
    [HideInInspector] public bool alt;

    [Header("Input (New)")]
    [HideInInspector]public CombatInput combatInput;          // assign in Inspector
    [Range(0.05f, 1.0f)] public float inputBufferWindow = 0.45f;

    private Coroutine _cooldownRoutine;

    // ================================
    //             AMMO
    // ================================
    private void InitWeaponAmmo()
    {
        if (data == null || !data.usesAmmo)
        {
            if (HUDController.instance != null)
                HUDController.instance.weaponAmmoUI.Hide();
            return;
        }

        if (HUDController.instance != null)
        {
            HUDController.instance.weaponAmmoUI.Show();
            HUDController.instance.weaponAmmoUI.SetAmmoPips(data.maxAmmo);
        }

        if (!instance.ammoInitialized)
        {
            instance.currentAmmo = (data.ammoOnStart >= 0)
                ? Mathf.Clamp(data.ammoOnStart, 0, data.maxAmmo)
                : data.maxAmmo;

            instance.regenBlockedUntil = 0f;
            instance.ammoFractionalCarry = 0f;
            instance.ammoInitialized = true;
        }
    }

    private void UpdateWeaponAmmoRegen(float dt)
    {
        if (data == null || !data.usesAmmo || instance == null) return;

        // hide/show weapon on empty if requested
        if (WeaponModel != null)
        {
            bool hide = data.HideWeaponWhenAmmoEmpty && instance.currentAmmo == 0;
            WeaponModel.SetActive(!hide);
        }

        // regen only when allowed
        if (data.regenOnlyWhenReadyState && State != AttackState.Ready) return;
        if (Time.time < instance.regenBlockedUntil) return;
        if (instance.currentAmmo >= data.maxAmmo) { instance.ammoFractionalCarry = 0f; return; }

        float toAdd = data.ammoRegenPerSecond * dt + instance.ammoFractionalCarry;
        int whole = Mathf.FloorToInt(toAdd);
        instance.ammoFractionalCarry = toAdd - whole;

        if (whole > 0)
            instance.currentAmmo = Mathf.Min(data.maxAmmo, instance.currentAmmo + whole);
    }

    public int GetWeaponAmmo() =>
        (data != null && data.usesAmmo && instance != null) ? instance.currentAmmo : 0;

    public int GetWeaponMaxAmmo() =>
        (data != null && data.usesAmmo) ? data.maxAmmo : 0;

    public float GetWeaponFractionalRegen() =>
        (data != null && data.usesAmmo && instance != null) ? instance.ammoFractionalCarry : 0;

    // ================================
    //           LIFECYCLE
    // ================================
    private void Awake()
    {
        if (Singleton != null && Singleton != this) { Destroy(gameObject); return; }
        Singleton = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        _playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
        State = AttackState.Ready;

        if (cam != null) IK_Handler = cam.GetComponentInChildren<HandIKHandler>();

        ComboCounter = 0;
        CurrentAttackData = null;

        if (data == null || instance == null)
        {
            enabled = false;
            return;
        }

        HandleWeaponSwapping(); // builds models + animator

        combatInput = gameObject.GetComponent<CombatInput>();

    }

    private void OnDisable()
    {
        // prevent stray cooldown coroutines dangling
        if (_cooldownRoutine != null) StopCoroutine(_cooldownRoutine);
    }

    private void Update()
    {
        UpdateStatusEffects();
        UpdateWeaponAmmoRegen(Time.deltaTime);
        ProcessBufferedInputs();


        if (State == AttackState.Charging && _awaitingReleaseIndex >= 0 && IndexValid(_awaitingReleaseIndex))
        {
            var logic = data.Weapon_Attacks[_awaitingReleaseIndex].weaponInputLogic;
            logic?.OnHoldTick(_awaitingReleaseIndex, this);
        }

#if UNITY_EDITOR
        // quick test keys
        if (Input.GetKey(KeyCode.Alpha9)) { State = AttackState.Ready; ComboCounter = 0; }
        if (Input.GetKeyDown(KeyCode.Alpha8)) instance.ModifierSlots[0].Equip(new LifestealEffectModifier(1f, 1f));
        if (Input.GetKeyDown(KeyCode.Alpha7)) instance.ModifierSlots[0].Equip(new FreezeEffectModifier(1f, 3f));
#endif
    }

    // ================================
    //        WEAPON SWAP / SPAWN
    // ================================
    public void SetWeaponInstance(WeaponInstance newInstance, bool unlockA2 = false, bool unlockA3 = false)
    {
        _isAttack2Unlocked = unlockA2;
        _isAttack3Unlocked = unlockA3;

        if (newInstance == instance) return;

        // drop current weapon if any
        if (instance != null && instance.data != null)
            SpawnWeapon(instance);

        instance = newInstance;
        data = instance != null ? instance.data : null;

        if (data == null) { enabled = false; return; }

        HandleWeaponSwapping();
    }

    public void SpawnWeapon(WeaponInstance weaponInstance)
    {
        var spawner = new GameObject("WeaponSpawner");
        spawner.transform.position = transform.position + playerMovement.orientation.transform.forward + Vector3.up;
        spawner.AddComponent<WeaponSpawner>().CreateWeapon(weaponInstance);
    }

    public void HandleWeaponSwapping()
    {
        // clear old models
        if (WeaponModel != null) Destroy(WeaponModel);
        if (SecondaryModel != null) Destroy(SecondaryModel);

        // primary
        Transform parent = (data.hand == WeaponDataSO.Hand.right) ? RhandPos : LhandPos;
        if (data.model != null && parent != null)
        {
            WeaponModel = Instantiate(data.model, parent);
            WeaponModel.transform.localPosition = Vector3.zero;
            WeaponModel.layer = 7; // your gameplay layer
        }

        // secondary
        if (data.secondaryModel != null && secondaryPos != null)
            SecondaryModel = Instantiate(data.secondaryModel, secondaryPos);

        // animator
        if (Weapon_anim != null && data.Anim_controller != null)
            Weapon_anim.runtimeAnimatorController = data.Anim_controller;

        // unlocks by kills
        _isAttack2Unlocked |= instance.KillCount >= instance.Threshold1;
        _isAttack3Unlocked |= instance.KillCount >= instance.Threshold2;

        // ammo
        InitWeaponAmmo();

        // reset charge
        ChargeAmount = 1f;
    }

    // ================================
    //           INPUT HANDLERS
    // ================================

    private void ProcessBufferedInputs()
    {
        if (combatInput == null || data == null) return;

        combatInput.bufferWindow = inputBufferWindow;
        combatInput.CleanupExpired();

        if (State == AttackState.Attacking && combatInput.TryPeekOldest(out var eAtt) && eAtt.phase == InputPhase.Pressed)
        {
            int i = (int)eAtt.action;
            if (IndexValid(i) && IsAttackUnlocked(i))
            {
                var ad = data.Weapon_Attacks[i];
                var logic = ad.weaponInputLogic;
                if (logic != null && logic.StartsChargingOnPress && ad.allowAttackInterupt && CanAttemptAttack(i))
                {
                    SoftInterruptCurrentAttack();                 // stop current swing (no cooldown)
                    logic.OnPress(i, this, eAtt.alt);           // this will call StartAttackCharging(idx)
                    combatInput.ConsumeOldest();
                    // Releases will be handled by DrainReleases()
                    DrainReleases();
                    return;
                }
            }
            // If not allowed, leave it buffered until Ready/Combo
        }

        // --- existing path: consume presses in Ready/Combo ---
        if (State == AttackState.Ready || State == AttackState.Combo)
            ConsumeOnePressIfAvailable();

        // Always clear releases at the end (routes to OnRelease only if Charging & matching)
        DrainReleases();
    }



    private bool IsAttackUnlocked(int i) =>
    i == 0 || (i == 1 && _isAttack2Unlocked) || (i == 2 && _isAttack3Unlocked);

    // Ensures we never consume more than one press in the same frame
    private int _lastPressConsumedFrame = -1;


    

    private bool ConsumeOnePressIfAvailable()
    {
        if (_lastPressConsumedFrame == Time.frameCount) return false;
        if (!combatInput.TryPeekOldest(out var e) || e.phase != InputPhase.Pressed) return false;

        int idx = (int)e.action;
        if (!IndexValid(idx) || !IsAttackUnlocked(idx)) { combatInput.ConsumeOldest(); return false; }
        if (!CanAttemptAttack(idx)) return false; // leave in buffer; try again later

        data.Weapon_Attacks[idx].weaponInputLogic.OnPress(idx, this, e.alt);
        combatInput.ConsumeOldest();
        _lastPressConsumedFrame = Time.frameCount;
        return true;
    }

    private void DrainReleases()
    {
        // Consume releases so they never block, and trigger OnRelease only when actually charging that attack
        while (combatInput.TryPeekOldest(out var head) && head.phase == InputPhase.Released)
        {
            int ai = (int)head.action;

            bool isChargingMatch =
                State == AttackState.Charging &&
                IndexValid(ai) &&
                CurrentAttackData == data.Weapon_Attacks[ai];

            if (isChargingMatch)
            {
                data.Weapon_Attacks[ai].weaponInputLogic.OnRelease(ai, this, head.alt);
            }

            combatInput.ConsumeOldest();
        }
    }

    private void TryConsumeForComboNow()
    {
        // 1) Try a press immediately (snappy chaining)
        ConsumeOnePressIfAvailable();
        // 2) Then drain releases so a just-finished charge can complete
        DrainReleases();
    }


    private bool IndexValid(int i) => data != null && data.Weapon_Attacks != null && i >= 0 && i < data.Weapon_Attacks.Count;

    private bool CanAttemptAttack(int attackIndex)
    {
        if (data == null || instance == null) return false;
        if (!IndexValid(attackIndex)) return false;

        if (!data.usesAmmo) return true;

        // Empty-lock: block all attacks when empty, if enabled
        if (data.blockAttacksWhenAmmoEmpty && instance.currentAmmo <= 0)
            return false;

        // Per-attack cost
        var nextData = data.Weapon_Attacks[attackIndex];
        if (nextData.ammoCost > 0 && instance.currentAmmo < nextData.ammoCost)
            return false;

        return true;
    }

    // ================================
    //        ATTACK FLOW / STATE
    // ================================
    public void EnterAttack(int i, bool isAlt = false)
    {
        if (!enabled || data == null || instance == null) return;
        if (!IndexValid(i)) return;
        var nextData = data.Weapon_Attacks[i];

        // spend ammo
        if (data.usesAmmo && nextData.ammoCost > 0)
        {
            instance.currentAmmo = Mathf.Max(0, instance.currentAmmo - nextData.ammoCost);
            instance.regenBlockedUntil = Time.time + data.ammoRegenDelay;
        }

        State = AttackState.Attacking;
        alt = isAlt;

        CurrentAttackData = nextData;
        playerMovement.AttackMoveSpeed(CurrentAttackData.MoveSpeedMult, CurrentAttackData.AllowAgility);
        CurrentAttackData.EnterAttack(this);

        // Anim params
        if (Weapon_anim != null)
        {
            Weapon_anim.SetBool("Alt", alt);
            Weapon_anim.SetInteger("AttackType", i);
            print("ComboCounter: " + ComboCounter);
            Weapon_anim.SetInteger("ComboInt", ComboCounter);

            Weapon_anim.SetBool("Attacking", true);
            Weapon_anim.SetBool("Charging", false);
        }

        // combo progress
        ComboCounter++;
        if (ComboCounter >= CurrentAttackData.ComboLength) ComboCounter = 0;
    }

    public void ExitAttack()
    {
        if (CurrentAttackData == null) return;

        playerMovement.AttackResetMoveSpeed();
        if (Weapon_anim != null)
        {
            Weapon_anim.SetBool("Attacking", false);
            Weapon_anim.SetBool("Alt", false);
            Weapon_anim.SetBool("Combo", false);
            Weapon_anim.SetBool("Charging", false);
        }

        HandleCooldownOrReady();
        CurrentAttackData.ExitAttack(this);
        CurrentAttackData = null;
        
        ChargeAmount = 1f;
    }

    private void HandleCooldownOrReady()
    {
        if (CurrentAttackData != null && CurrentAttackData.hasCooldown)
        {
            State = AttackState.Cooldown;
            if (_cooldownRoutine != null) StopCoroutine(_cooldownRoutine);
            _cooldownRoutine = StartCoroutine(Cooldown(CurrentAttackData.cooldown));
        }
        else
        {
            State = AttackState.Ready;
        }
    }

    private IEnumerator Cooldown(float t)
    {
        yield return new WaitForSeconds(t);
        if (State != AttackState.Charging) State = AttackState.Ready;
        ComboCounter = 0;             
        _cooldownRoutine = null;
    }

    private void SoftInterruptCurrentAttack()
    {
        // Clean up current attack without putting you into cooldown
        playerMovement.AttackResetMoveSpeed();

        if (Weapon_anim != null)
        {
            Weapon_anim.SetBool("Attacking", false);
            Weapon_anim.SetBool("Combo", false);
            // Don't touch "Charging" here; OnPress will set it
        }

        // If you have custom cleanup per attack, add a hook:
        // CurrentAttackData?.OnInterrupted(this);

        CurrentAttackData = null;
        State = AttackState.Ready; // so the next OnPress can proceed safely
    }

    // ================================
    //           ANIM EVENTS
    // ================================
    public void AttackPerformed() => CurrentAttackData?.PerformAttack(this);

    public void OpenComboWindow()
    {
        State = AttackState.Combo;
        if (Weapon_anim != null) Weapon_anim.SetBool("Combo", true);

        // Try to chain instantly this frame; per-frame guard prevents double consumption
        TryConsumeForComboNow();
    }



    public void CloseComboWindow()
    {
        State = AttackState.Attacking;
        if (Weapon_anim != null) Weapon_anim.SetBool("Combo", false);
    }

    private int _awaitingReleaseIndex = -1;
    private float _awaitingSetTime;

    public void StartAttackCharging(int attackIndex)
    {
        if (!IndexValid(attackIndex)) return;

        _awaitingReleaseIndex = attackIndex;
        _awaitingSetTime = Time.time;

        CurrentAttackData = data.Weapon_Attacks[attackIndex];
        State = AttackState.Charging;
        ChargeAmount = 0f;
        Debug.Log("AttackCharging");
        playerMovement.AttackMoveSpeed(CurrentAttackData.MoveSpeedMult, CurrentAttackData.AllowAgility);

        if (Weapon_anim != null)
        {
            Weapon_anim.SetBool("Charging", true);
            Weapon_anim.SetInteger("AttackType", attackIndex);
        }
    }





    public void AttackForce(int i)
    {
        if (playerMovement.state == PlayerMovement.PlayerMechanimState.Jumping ||
            playerMovement.state == PlayerMovement.PlayerMechanimState.Sliding) return;

        CurrentAttackData?.ApplyForceToPlayer(this, i);
    }

    public void EnemyKilled()
    {
        if (instance == null) return;
        instance.KillCount++;
        if (instance.KillCount >= instance.Threshold1) _isAttack2Unlocked = true;
        if (instance.KillCount >= instance.Threshold2) _isAttack3Unlocked = true;
    }

    // ================================
    //        DAMAGE / EFFECTS
    // ================================
    public float DamageBonus(DamageType damageType)
    {
        float bonus = PlayerStats.instance.GetStat(StatType.AllDamageBuff);
        switch (damageType)
        {
            case DamageType.Umbraveil: bonus += PlayerStats.instance.GetStat(StatType.UmbravailDamageBuff); break;
            case DamageType.Scarforge: bonus += PlayerStats.instance.GetStat(StatType.ScarForgeDamageBuff); break;
            case DamageType.Verdancy: bonus += PlayerStats.instance.GetStat(StatType.VerdancyDamageBuff); break;
            case DamageType.Aetherflow: bonus += PlayerStats.instance.GetStat(StatType.AetherflowDamageBuff); break;
        }
        return bonus / 100f;
    }

    public void SpawnVFX(int i) => PlayerVFXHandler.instance?.SpawnVFX(CurrentAttackData?.VFX, i);

    public GameObject SpawnObject(GameObject go, Vector3 pos, Quaternion rot, Transform parent = null) { GameObject inst = Instantiate(go, pos, rot, parent); return inst; }

    public void TriggerBlock(bool tf) =>
        SetPlayerHealthState(tf ? PlayerHealth.DamageState.Blocking : PlayerHealth.DamageState.Normal);

    public void ParryWindow(bool tf)
    {
        if (tf)
        {
            if (CurrentAttackData is Weapon_Attack_Data_BlockParry b) b.Parry(this);
            SetPlayerHealthState(PlayerHealth.DamageState.Parrying);
        }
        else
        {
            SetPlayerHealthState(PlayerHealth.DamageState.Normal);
        }
    }

    public void SetPlayerHealthState(PlayerHealth.DamageState state) =>
        PlayerHealth.instance.d_state = state;

    public void ApplyEffectToPlayer(statusEffectBase eff)
    {
        if (eff == null) return;
        _activeEffects.Add(eff);
        eff.StartEffect(gameObject);
    }

    public void ApplyEffectToEnemy(statusEffectBase eff, DamagableEnemy enemy)
    {
        if (eff == null || enemy == null) return;
        _activeEffects.Add(eff);
        eff.StartEffect(enemy.gameObject);
    }

    public void UpdateStatusEffects()
    {
        float dt = Time.deltaTime;
        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            var eff = _activeEffects[i];
            if (eff == null) { _activeEffects.RemoveAt(i); continue; }

            if (eff.IsEffectDone())
            {
                _activeEffects.RemoveAt(i);
                continue;
            }
            eff.UpdateEffect(dt);
        }
    }
}
