using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField]
    private float _moveSpeed, _groundDrag, _airDrag, _airMoveMultiplier, _groundMoveMultiply, _jumpForce;
    private float _currentMoveSpeed;
    public Transform orientation;

    public float jumpEnhance = 1f;

    public LayerMask whatisGround;

    float horMovement;
    float vertMovement;

    float _moveMultiply;


    public bool isGrounded;
    public float MaxSlopeAngle;
    RaycastHit slopeHit;

    Vector3 slopeMoveDirection;
    Vector3 MoveDir;

    Rigidbody rb;
    CamAttackAnim CamattackAnim;

    private void Start()
    {
        CamattackAnim = Camera.main.GetComponentInParent<CamAttackAnim>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _currentMoveSpeed = _moveSpeed;
    }

    public void SetAttackMoveSpeed(float mult)
    {
        _currentMoveSpeed = mult*_moveSpeed;
    }

    public void ResetMoveSpeed()
    {
        _currentMoveSpeed = _moveSpeed;
    }


    private void Update()
    {
        if (!isGrounded)
        {
            isGrounded = Physics.CheckSphere(transform.position + 0.3f * Vector3.up, 0.4f, whatisGround);
            if (isGrounded) CamattackAnim.RotateCamera(Vector2.down, 0.8f);
        }
        isGrounded = Physics.CheckSphere(transform.position+0.3f*Vector3.up, 0.4f, whatisGround);

        MyInput();
        ControlDrag();

        if (OnSlope())
        {

            slopeMoveDirection = Vector3.ProjectOnPlane(MoveDir, slopeHit.normal);

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

        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
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
            rb.linearDamping = _groundDrag;
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
        CamattackAnim.RotateCamera(Vector2.up, 0.7f);
        rb.AddForce(_jumpForce * transform.up * jumpEnhance, ForceMode.Impulse);
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
            
            rb.AddForce((1-slopeMoveDirection.magnitude) * -1 * Vector3.ProjectOnPlane(Physics.gravity, slopeHit.normal), ForceMode.Acceleration);
            
        }

    }
}
