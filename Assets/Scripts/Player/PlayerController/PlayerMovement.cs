using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField]
    private float _moveSpeed, _groundDrag, _airDrag, _airMoveMultiplier, _groundMoveMultiply, _jumpForce;
    public Transform orientation;


    public LayerMask whatisGround;

    float horMovement;
    float vertMovement;

    float _moveMultiply;


    bool isGrounded;

    RaycastHit slopeHit;

    Vector3 slopeMoveDirection;
    Vector3 MoveDir;

    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }


    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position+0.3f*Vector3.up, 0.4f, whatisGround);


        MyInput();
        ControlDrag();

        if (OnSlope())
        {
            slopeMoveDirection = Vector3.ProjectOnPlane(MoveDir, slopeHit.normal);
        }

    }
    void MyInput()
    {
        horMovement = Input.GetAxisRaw("Horizontal");
        vertMovement = Input.GetAxisRaw("Vertical");

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
        rb.AddForce(_jumpForce * transform.up, ForceMode.Impulse);
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
            rb.AddForce(MoveDir * _moveSpeed * _moveMultiply, ForceMode.Acceleration);
        }
        else if(isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection * _moveSpeed * _moveMultiply, ForceMode.Acceleration);
        }

    }
}
