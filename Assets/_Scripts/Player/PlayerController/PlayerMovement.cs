using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerMovement : MonoBehaviour
{



    [Header("General Controls")]
    [SerializeField]
    private float MaxMoveSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _CrouchMoveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundDrag;
    [SerializeField] private float _airDrag;
    [SerializeField] private float _airMoveMultiplier;
    [SerializeField] private float _groundMoveMultiply;

    private float _currentMoveSpeed;

    [Header("SetUp")]
    public float MaxSlopeAngle;
    public float jumpEnhance = 1f;
    public float SprintMult = 1.5f;
    public LayerMask whatisGround;
    public Transform orientation;
    public bool isGrounded;
    float horMovement;
    float vertMovement;

    float _moveMultiply;


    
    
    RaycastHit slopeHit;

    Vector3 slopeMoveDirection;
    Vector3 MoveDir;
    CapsuleCollider collider;
    float ColliderHeight;
    Rigidbody rb;
    CamAttackAnim CamattackAnim;
    
    
    [Header("Sliding")]
    
    public float SlideTime;
    public float SlideDrag;
    public float SlideForce;
    public float SlideThresh;
    public float VelocityThresh;
    public float LandingSlide;

    [Header("Misc")]
    public AudioSource WindAS;
    public float VolLerpSpeed;
    public float MaxVol;
    public Transform headPos;
    public float CamFXSpeed;
    public float SlidingCamRot;
    public float MaxSlidingCamRot;

    float timer;
    public enum PlayerMechanimState
    {
        Walking,
        Sprinting,
        Jumping,
        Crouching,
        Sliding
    }

    public PlayerMechanimState state;
    public WeaponHolder wh;
    private void Start()
    {
        wh = GetComponent<WeaponHolder>();
        collider = GetComponent<CapsuleCollider>();
        ColliderHeight = collider.height;
        CamattackAnim = Camera.main.GetComponentInParent<CamAttackAnim>();
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = MaxMoveSpeed;
        rb.freezeRotation = true;
        _currentMoveSpeed = _moveSpeed;
    }

    public void SetMoveSpeed(float mult)
    {
        _currentMoveSpeed = mult*_moveSpeed;
    }

    public void ResetMoveSpeed()
    {
        
        _currentMoveSpeed = _moveSpeed;
    }

    

    private void Update()
    {
        Grounding();

        MyInput();
        ControlDrag();
        WindSFX();
        if (OnSlope())
        {

            slopeMoveDirection = Vector3.ProjectOnPlane(MoveDir, slopeHit.normal);

        }
        CamRotation();
    }

    private void CamRotation()
    {
        Quaternion target_Rot = Quaternion.identity;
        if (state == PlayerMechanimState.Sliding)
        {
             
            float RotationMag = Vector3.Dot(orientation.right, rb.linearVelocity);

            target_Rot = Quaternion.Euler(0, 0, Mathf.Clamp(RotationMag * SlidingCamRot, -MaxSlidingCamRot, MaxSlidingCamRot));
            headPos.transform.rotation = Quaternion.Lerp(headPos.transform.localRotation, target_Rot, Time.deltaTime * CamFXSpeed);
        }
        
        
    }

    public void Grounding()
    {
        if (!isGrounded)
        {
            isGrounded = Physics.CheckSphere(transform.position + 0.3f * Vector3.up, 0.4f, whatisGround);
            if (isGrounded) // OnLanding
            {
                state = PlayerMechanimState.Walking;
                if (Input.GetKey(KeyCode.C))
                {
                    //state = PlayerMovement.PlayerMechanimState.Sliding;
                    //Slide();
                    Vector3 vel = Vector3.ProjectOnPlane(rb.linearVelocity, slopeHit.normal).normalized;
                    rb.AddForce(vel * LandingSlide, ForceMode.VelocityChange);
                }
                CamattackAnim.RotateCamera(Vector2.down, 0.8f);
                PlayerSoundSource.instance.PlaySound(PlayerSoundSource.SoundType.FootSteps, Mathf.Clamp(1*rb.linearVelocity.magnitude,0.5f,1.5f));
            }
        }
        else
        {
            isGrounded = Physics.CheckSphere(transform.position + 0.3f * Vector3.up, 0.4f, whatisGround);
        }
    }


    [HideInInspector] public Vector2 moveInput;

    void MyInput()
    {
        horMovement = Input.GetAxisRaw("Horizontal");
        vertMovement = Input.GetAxisRaw("Vertical");
        moveInput.x = horMovement;
        moveInput.y = vertMovement;
        moveInput = moveInput.normalized;
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))//Jumping
        {
            state = PlayerMechanimState.Jumping;
            Jump();
        }
        //Sprinting
        else if (isGrounded && Input.GetKey(KeyCode.LeftShift) && state != PlayerMechanimState.Crouching && state != PlayerMechanimState.Sliding && isAttackAgile())
        {
            state = PlayerMechanimState.Sprinting;
            SetMoveSpeed(SprintMult);
        }
        if (isGrounded && Input.GetKey(KeyCode.C))
        {
            if (rb.linearVelocity.magnitude > VelocityThresh && state != PlayerMechanimState.Crouching) //Sliding
            {
                state = PlayerMechanimState.Sliding;
                Slide();
            }
            else //Crouching
            {
                
                Crouch();
            }
        }
        else if (isGrounded && Input.GetKeyUp(KeyCode.C))
        {
            state = PlayerMechanimState.Walking;
            EndCrouch();
        }
        else
        {
            state = PlayerMovement.PlayerMechanimState.Walking;
        }
        if (!isGrounded)
        {
            state = PlayerMechanimState.Jumping;
        }
        
        
        
        MoveDir = (orientation.forward * vertMovement + orientation.right * horMovement).normalized;
        if (Vector3.Dot(rb.linearVelocity, MoveDir) < 0) MoveDir = 2 * MoveDir;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            if(state == PlayerMechanimState.Sliding)
            {
                rb.linearDamping = SlideDrag;
            }
            else rb.linearDamping = _groundDrag;
            _moveMultiply = _groundMoveMultiply;
        }
        else
        {
            rb.linearDamping = _airDrag;
            _moveMultiply = _groundMoveMultiply * _airMoveMultiplier;
        }
    }
    
    void Jump()
    {
        Vector3 vel = rb.linearVelocity;
        vel.y = 0;
        rb.linearVelocity = vel;
        PlayerSoundSource.instance.PlaySound(PlayerSoundSource.SoundType.FootSteps, 1);
        CamattackAnim.RotateCamera(Vector2.up, 0.7f);
        rb.AddForce(_jumpForce * Vector3.up * jumpEnhance, ForceMode.Impulse);
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                
                return true;
                
            }
            else
            {
                
                return false;
            }
        }
        else
        {
            return false;
        }
    }



    void MovePlayer()
    {
        if (!OnSlope())
        {
            rb.AddForce(MoveDir * _currentMoveSpeed * (1 + 0.01f*PlayerStats.instance.GetStat(StatType.MoveSpeedIncrease)) * _moveMultiply, ForceMode.Acceleration);
        }
        else if(isGrounded && OnSlope())
        {
            float SlopeAngle = Vector3.Angle(slopeHit.normal, Vector3.up);
            //Debug.Log(SlopeAngle);
            if (SlopeAngle > MaxSlopeAngle)
            {
                
                return;
            }
            rb.AddForce(slopeMoveDirection * _currentMoveSpeed * (1 + 0.01f * PlayerStats.instance.GetStat(StatType.MoveSpeedIncrease)) * _moveMultiply, ForceMode.Acceleration);
            
            //rb.AddForce((1-slopeMoveDirection.magnitude) * -1 * Vector3.ProjectOnPlane(Physics.gravity, slopeHit.normal), ForceMode.Acceleration);
            
        }

    }


    public void Slide()
    {
        if (timer > SlideTime)
        {
            Crouch();
            return;
        }
        SetMoveSpeed(0);
        collider.height = 0.5f * ColliderHeight;
        rb.linearDamping = 0;
        timer += Time.deltaTime;
        if(timer < SlideThresh)
        {
            rb.AddForce(rb.linearVelocity * SlideForce*Time.deltaTime);
        }



    }

    public void Crouch()
    {
        state = PlayerMechanimState.Crouching;
        rb.linearDamping = _groundDrag;
        collider.height = 0.5f * ColliderHeight;
        SetMoveSpeed(_CrouchMoveSpeed);
    }

    public void EndCrouch()
    {
        timer = 0;
        ResetMoveSpeed();
        collider.height = ColliderHeight;
    }



    public void WindSFX()
    {
        if (!isGrounded)
        {
            WindAS.volume = Mathf.Lerp(WindAS.volume, Mathf.Clamp(rb.linearVelocity.magnitude / 10, 0, MaxVol), VolLerpSpeed * Time.deltaTime);
        }
        else
        {
            WindAS.volume = 0;
        }
    }

    bool AttackAgile = true;

    public void AttackMoveSpeed(float val,bool Agile)
    {
        if (state == PlayerMechanimState.Sprinting)
        {
            state = PlayerMechanimState.Walking;
        }
        SetMoveSpeed(val);
        AttackAgile = Agile;
    }

    public void AttackResetMoveSpeed()
    {
        AttackAgile = true;
        ResetMoveSpeed();
    }

    private bool isAttackAgile()
    {
        return AttackAgile;
    }
}
