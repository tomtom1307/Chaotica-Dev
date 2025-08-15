using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // ================================
    //            CONFIG
    // ================================
    [Header("General Controls")]
    [SerializeField] private float MaxMoveSpeed = 10f;
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float _CrouchMoveSpeed = 3.5f;
    [SerializeField] private float _jumpForce = 6.5f;
    [SerializeField] private float _groundDrag = 5f;
    [SerializeField] private float _airDrag = 0.5f;
    [SerializeField] private float _airMoveMultiplier = 0.8f;
    [SerializeField] private float _groundMoveMultiply = 1.0f;

    [Header("Setup")]
    public float MaxSlopeAngle = 50f;
    public float jumpEnhance = 1f;
    public float SprintMult = 1.5f;
    public LayerMask whatisGround;
    public Transform orientation;
    public Transform headPos;

    [Header("Sliding")]
    public float SlideTime = 1.0f;
    public float SlideDrag = 0.2f;
    public float SlideForce = 60f;
    public float SlideThresh = 0.25f;
    public float VelocityThresh = 6f;     // legacy threshold
    public float LandingSlide = 2.5f;
    public float SlideMovementControl = 1.1f; // temporary move speed while sliding

    // Slide hysteresis to avoid rapid enter/exit at threshold
    [SerializeField] private float slideEnterSpeed = 7.5f;
    [SerializeField] private float slideExitSpeed = 6.0f;

    [Header("Camera & FOV")]
    public float CamFXSpeed = 10f;
    public float SlidingCamRot = 0.8f;
    public float MaxSlidingCamRot = 15f;
    [SerializeField] private float sprintFOV = +5f;
    [SerializeField] private float slideFOV = +7f;
    [SerializeField] private float crouchFOV = -3f;

    [Header("SFX")]
    public AudioSource WindAS;
    public float VolLerpSpeed = 8f;
    public float MaxVol = 0.75f;
    public AudioSource SlidingAS;
    public float SlideVolLerpSpeed = 10f;
    public float SlideMaxVol = 0.9f;

    [Header("Grounding Check")]
    public Vector3 groundSphereOffset = new Vector3(0, -0.9f, 0);
    public float groundSphereRadius = 0.25f;

    // ================================
    //           RUNTIME STATE
    // ================================
    public enum PlayerMechanimState { Walking, Sprinting, Jumping, Crouching, Sliding }
    public PlayerMechanimState state;

    public bool isGrounded { get; private set; }


    // --- Jump Assist ---
    [Header("Jump Assist")]
    [SerializeField] private float coyoteTime = 0.12f;      // grace after leaving ground
    [SerializeField] private float jumpBufferTime = 0.12f;   // press jump slightly early
    private float _coyoteTimer;
    private float _jumpBufferTimer;


    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Vector3 MoveDir;
    [HideInInspector] public Vector3 slopeMoveDirection;

    private float _currentMoveSpeed;
    private float _moveMultiply;
    private float _slideTimer;
    private bool _onSlopeCached;
    private bool _applyJumpImpulseThisFrame;
    public bool OnSlope() => _onSlopeCached;

    private CapsuleCollider _collider;
    private float _colliderHeight;
    private Rigidbody _rb;
    private CamAttackAnim _camAttackAnim;
    private RaycastHit _slopeHit;

    // Attack movement gate
    bool AttackAgile = true;
    float attackMoveSpeed = 1f;

    // cached inputs
    float _hor, _ver;

    private void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rb = GetComponent<Rigidbody>();
        _camAttackAnim = Camera.main.GetComponentInParent<CamAttackAnim>();

        _colliderHeight = _collider.height;
        _rb.maxLinearVelocity = MaxMoveSpeed;   // assuming custom extension
        _rb.freezeRotation = true;

        _currentMoveSpeed = _moveSpeed;
        state = PlayerMechanimState.Walking;
    }

    private void Update()
    {
        UpdateGrounded();
        CaptureInput();
        ResolveState();
        UpdateJumpAssistTimers();
        UpdateCameraTilt();
        UpdateWindSFX();
        UpdateSlidingSFX();

        // Cache slope info once per frame
        _onSlopeCached = CheckSlope(out _slopeHit);
        if (_onSlopeCached)
            slopeMoveDirection = Vector3.ProjectOnPlane(MoveDir, _slopeHit.normal);
    }

    private void FixedUpdate()
    {
        ControlDrag();
        SlideUpdateIfNeeded();       // per-frame slide force/timer while in Sliding
        MovePlayer();
    }

    // ================================
    //            INPUT
    // ================================
    private struct InputSnapshot
    {
        public Vector2 move;
        public bool jumpPressed;
        public bool sprintHeld;
        public bool crouchHeld;
        public bool crouchReleased;
    }

    private InputSnapshot _inp;

    private void CaptureInput()
    {
        _hor = Input.GetAxisRaw("Horizontal");
        _ver = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(_hor, _ver).normalized;

        _inp = new InputSnapshot
        {
            move = moveInput,
            jumpPressed = Input.GetKeyDown(KeyCode.Space),
            sprintHeld = Input.GetKey(KeyCode.LeftShift),
            crouchHeld = Input.GetKey(KeyCode.C),
            crouchReleased = Input.GetKeyUp(KeyCode.C)
        };

        // direction
        MoveDir = (orientation.forward * _inp.move.y + orientation.right * _inp.move.x).normalized;
        if (Vector3.Dot(_rb.linearVelocity, MoveDir) < 0) MoveDir *= 2f;
    }

    // ================================
    //          STATE MACHINE
    // ================================
    private void ResolveState()
    {
        // Want to jump if both buffers are alive
        bool wantsJump = (_jumpBufferTimer > 0f) && (_coyoteTimer > 0f);

        bool hasMoveInput = _inp.move.sqrMagnitude > 0.0001f;

        bool canSprint = isGrounded
                         && hasMoveInput
                         && state != PlayerMechanimState.Crouching
                         && state != PlayerMechanimState.Sliding
                         && isAttackAgile()
                         && attackMoveSpeed >= 1f;

        bool wantsSprint = canSprint && _inp.sprintHeld;

        float speed = _rb.linearVelocity.magnitude;
        bool slideAllowedNow = (state == PlayerMechanimState.Sliding)
                               ? speed > slideExitSpeed
                               : speed > slideEnterSpeed;

        bool wantsSlide = isGrounded && _inp.crouchHeld && slideAllowedNow && state != PlayerMechanimState.Crouching;
        bool wantsCrouch = isGrounded && _inp.crouchHeld && !wantsSlide;

        PlayerMechanimState next = state;

        // IMPORTANT: set the impulse flag ONLY when the jump is intentional
        _applyJumpImpulseThisFrame = false;

        if (wantsJump)
        {
            next = PlayerMechanimState.Jumping;
            _applyJumpImpulseThisFrame = true;   // <- only true for buffered/intentional jump
        }
        else if (!isGrounded)
        {
            next = PlayerMechanimState.Jumping;  // falling, NO impulse
        }
        else if (wantsSlide)
        {
            next = PlayerMechanimState.Sliding;
        }
        else if (wantsCrouch)
        {
            next = PlayerMechanimState.Crouching;
        }
        else if (wantsSprint)
        {
            next = PlayerMechanimState.Sprinting;
        }
        else
        {
            next = PlayerMechanimState.Walking;
        }

        // Manual uncrouch
        if (state == PlayerMechanimState.Crouching && _inp.crouchReleased)
            next = PlayerMechanimState.Walking;

        if (next != state)
        {
            OnExitState(state, next);
            SetState(next);
            OnEnterState(next, _applyJumpImpulseThisFrame); // pass the flag in
        }

        // consume jump buffers ONLY when we actually jumped
        if (_applyJumpImpulseThisFrame)
        {
            _jumpBufferTimer = 0f;
            _coyoteTimer = 0f;
        }

        // FOV channels every frame (stacking)
        FOVFXController.instance.SetOffset(FovChannel.Sprint, state == PlayerMechanimState.Sprinting ? sprintFOV : 0f);
        FOVFXController.instance.SetOffset(FovChannel.Slide, state == PlayerMechanimState.Sliding ? slideFOV : 0f);
        FOVFXController.instance.SetOffset(FovChannel.Crouch, state == PlayerMechanimState.Crouching ? crouchFOV : 0f);


#if UNITY_EDITOR
        DebugSprintGate(wantsSprint, canSprint);
#endif
    }

#if UNITY_EDITOR
    void DebugSprintGate(bool wantsSprint, bool canSprint)
    {

            Debug.Log(
                $"[Sprint Gate] canSprint={canSprint}, " +
                $"isGrounded={isGrounded}, hasMove={(_inp.move.sqrMagnitude > 0.0001f)}, " +
                $"state={state}, AttackAgile={AttackAgile}, attackMoveSpeed={attackMoveSpeed}"
            );
        
    }
#endif

    private void OnEnterState(PlayerMechanimState s, bool applyJumpImpulse = false)
    {
        switch (s)
        {
            case PlayerMechanimState.Jumping:
                if (applyJumpImpulse) DoJump();  // <- only jump when intended
                break;

            case PlayerMechanimState.Sprinting:
                SetMoveSpeed(SprintMult);
                break;

            case PlayerMechanimState.Sliding:
                SlideEnter();
                break;

            case PlayerMechanimState.Crouching:
                CrouchEnter();
                break;
        }
    }


    // now receives both 'from' and 'to' so we can fix the collider bug
    private void OnExitState(PlayerMechanimState from, PlayerMechanimState to)
    {
        switch (from)
        {
            case PlayerMechanimState.Sprinting:
                // If we’re going to walking (or anything that doesn’t set speed on enter),
                // reset to base speed.
                if (to == PlayerMechanimState.Walking)
                    ResetMoveSpeed();
                break;

            case PlayerMechanimState.Crouching:
                CrouchExit();
                break;

            case PlayerMechanimState.Sliding:
                SlideExit(to);
                break;
        }
    }



    private void SetState(PlayerMechanimState s) => state = s;

    // ================================
    //          MOVEMENT / PHYSICS
    // ================================
    private void ControlDrag()
    {
        if (isGrounded)
        {
            _rb.linearDamping = (state == PlayerMechanimState.Sliding) ? SlideDrag : _groundDrag;
            _moveMultiply = _groundMoveMultiply;
        }
        else
        {
            _rb.linearDamping = _airDrag;
            _moveMultiply = _groundMoveMultiply * _airMoveMultiplier;
        }
    }

    private void MovePlayer()
    {
        if (!_onSlopeCached)
        {
            _rb.AddForce(MoveDir * _currentMoveSpeed * (1 + 0.01f * PlayerStats.instance.GetStat(StatType.MoveSpeedIncrease)) * _moveMultiply, ForceMode.Acceleration);
        }
        else if (isGrounded)
        {
            float slopeAngle = Vector3.Angle(_slopeHit.normal, Vector3.up);
            if (slopeAngle > MaxSlopeAngle) return;

            _rb.AddForce(slopeMoveDirection * _currentMoveSpeed * (1 + 0.01f * PlayerStats.instance.GetStat(StatType.MoveSpeedIncrease)) * _moveMultiply, ForceMode.Acceleration);
        }
    }

    // ================================
    //            JUMP
    // ================================
    private void DoJump()
    {
        Vector3 vel = _rb.linearVelocity;
        vel.y = 0;
        _rb.linearVelocity = vel;

        PlayerSoundSource.instance.PlaySound(PlayerSoundSource.SoundType.FootSteps, 1);
        _camAttackAnim.RotateCamera(Vector2.up, 0.7f);
        _rb.AddForce(_jumpForce * Vector3.up * jumpEnhance, ForceMode.Impulse);
    }

    private void UpdateJumpAssistTimers()
    {
        // refresh coyote when grounded, tick down otherwise
        if (isGrounded) _coyoteTimer = coyoteTime;
        else _coyoteTimer -= Time.deltaTime;

        // set buffer on edge press, tick down
        if (_inp.jumpPressed) _jumpBufferTimer = jumpBufferTime;
        else _jumpBufferTimer -= Time.deltaTime;
    }


    // ================================
    //        CROUCH / SLIDE
    // ================================
    private void CrouchEnter()
    {
        // If we were sliding, play stop sound
        if (state == PlayerMechanimState.Sliding)
            PlayerSoundSource.instance.PlaySound(PlayerSoundSource.SoundType.SlideStop, SlideMaxVol, false, 1);

        _rb.linearDamping = _groundDrag;
        _collider.height = 0.5f * _colliderHeight;
        SetMoveSpeed(_CrouchMoveSpeed * attackMoveSpeed);
    }

    private void CrouchExit()
    {
        _slideTimer = 0f;
        AttackMoveSpeed(attackMoveSpeed, true);
        _collider.height = _colliderHeight;   // ensure full height on ANY crouch exit
    }

    private void SlideEnter()
    {
        _slideTimer = 0f;
        _collider.height = 0.5f * _colliderHeight;
        _rb.linearDamping = 0f;
        SetMoveSpeed(SlideMovementControl);
    }

    private void SlideUpdateIfNeeded()
    {
        if (state != PlayerMechanimState.Sliding) return;
        
        _slideTimer += Time.deltaTime;
        SetMoveSpeed(SlideMovementControl);
        if (_slideTimer > SlideTime)
        {
            // proper transition: slide -> crouch
            OnExitState(PlayerMechanimState.Sliding, PlayerMechanimState.Crouching);
            SetState(PlayerMechanimState.Crouching);
            OnEnterState(PlayerMechanimState.Crouching);
            return;
        }

        if (_slideTimer < SlideThresh)
        {
            _rb.AddForce(_rb.linearVelocity * (SlideForce * Time.deltaTime), ForceMode.Acceleration);
        }
    }

    private void SlideExit(PlayerMechanimState to)
    {
        // If we are not going into crouch, restore collider height here.
        if (to != PlayerMechanimState.Crouching)
        {
            _collider.height = _colliderHeight;
            _slideTimer = 0f;
            AttackMoveSpeed(attackMoveSpeed, true);
            _rb.linearDamping = isGrounded ? _groundDrag : _airDrag;
        }
        // If we go into crouch, CrouchEnter() will keep half height.
    }

    // ================================
    //          GROUNDING / SLOPE
    // ================================
    private void UpdateGrounded()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(transform.position + groundSphereOffset, groundSphereRadius, whatisGround);

        if (!wasGrounded && isGrounded)
        {
            // Landing
            state = PlayerMechanimState.Walking;

            if (Input.GetKey(KeyCode.C))
            {
                Vector3 vel = Vector3.ProjectOnPlane(_rb.linearVelocity, _slopeHit.normal).normalized;
                _rb.AddForce(vel * LandingSlide, ForceMode.VelocityChange);
            }

            _camAttackAnim.RotateCamera(Vector2.down, 0.8f);
            PlayerSoundSource.instance.PlaySound(PlayerSoundSource.SoundType.FootSteps, Mathf.Clamp(1 * _rb.linearVelocity.magnitude, 0.5f, 1.5f));
        }
    }

    private bool CheckSlope(out RaycastHit hit)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
            return hit.normal != Vector3.up;
        return false;
    }

    // ================================
    //            CAMERA FX
    // ================================
    private void UpdateCameraTilt()
    {
        if (state != PlayerMechanimState.Sliding)
        {
            headPos.transform.localRotation = Quaternion.Lerp(headPos.transform.localRotation, Quaternion.identity, Time.deltaTime * CamFXSpeed);
            return;
        }

        float rotationMag = Vector3.Dot(orientation.right, _rb.linearVelocity);
        Quaternion targetRot = Quaternion.Euler(0, 0, Mathf.Clamp(rotationMag * SlidingCamRot, -MaxSlidingCamRot, MaxSlidingCamRot));
        headPos.transform.localRotation = Quaternion.Lerp(headPos.transform.localRotation, targetRot, Time.deltaTime * CamFXSpeed);
    }

    // ================================
    //               SFX
    // ================================
    private void UpdateWindSFX()
    {
        if (!isGrounded)
        {
            float target = Mathf.Clamp(Mathf.Pow((_rb.linearVelocity.magnitude), 2) / 10f, 0, MaxVol);
            WindAS.volume = Mathf.Lerp(WindAS.volume, target, VolLerpSpeed * Time.deltaTime);
        }
        else
        {
            WindAS.volume = 0;
        }
    }

    private void UpdateSlidingSFX()
    {
        if (state == PlayerMechanimState.Sliding)
        {
            float target = Mathf.Pow(_rb.linearVelocity.magnitude, 1) / 5f;
            SlidingAS.volume = Mathf.Clamp(Mathf.Lerp(SlidingAS.volume, target, SlideVolLerpSpeed * Time.deltaTime), 0, SlideMaxVol);
        }
        else
        {
            SlidingAS.volume = 0;
        }
    }

    // ================================
    //        SPEED & ATTACK GATES
    // ================================
    public void SetMoveSpeed(float mult) => _currentMoveSpeed = mult * _moveSpeed;
    public void ResetMoveSpeed() => _currentMoveSpeed = _moveSpeed;

    public void AttackMoveSpeed(float val, bool Agile)
    {
        SetMoveSpeed(val);
        attackMoveSpeed = val;
        AttackAgile = Agile;
    }

    public void AttackResetMoveSpeed()
    {
        attackMoveSpeed = 1f;
        AttackAgile = true;
        ResetMoveSpeed();
    }

    private bool isAttackAgile() => AttackAgile;

    
}
