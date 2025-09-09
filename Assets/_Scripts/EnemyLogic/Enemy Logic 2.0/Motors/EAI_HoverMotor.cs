using UnityEngine;

[DefaultExecutionOrder(50)]
public class HoverMotor3D : MonoBehaviour, IGoalMotor
{
    [Header("Motion")]
    public float maxSpeed = 8f;
    public float accel = 25f;              
    public float arriveRadius = 0.5f;       

    [Header("Altitude Hold")]
    public float defaultAltitude = 3.0f;    
    public bool useGroundRef = true;
    public LayerMask groundMask = ~0;

    [Header("PID-Vert")]
    public float MaxI = 1;
    public float Kp;
    public float Ki;
    public float Kd;
    public float MaxAccel;
    


    // IMotor
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public bool Enabled { get => enabled; set => enabled = value; }

    // IGoalMotor state
    public bool ReachedGoal
    {
        get
        {
            return Vector3.Distance(transform.position, _goal)< arriveRadius;
        }
    }
    public Vector3 Destination => _goal;

    Rigidbody _rb;
    public float TargetAlt;
    float _CurrentAlt;
    Vector3 _goal;

    Vector3 _vel;              // kinematic velocity if no RB

    void Awake()
    {
        _rb = GetComponent<Rigidbody>(); // optional
        //_rb.maxLinearVelocity = maxSpeed;
        _goal = transform.position;
    }

    void FixedUpdate()
    {
        if (!Enabled) return;

        //Vector3 dir = (_goal-transform.position).normalized;
        
        Vector3 ay = VerticalPID();
        Vector3 Aflat = HorizontalPID();
        Vector3 dir = ay + Aflat;
        _rb.AddForce(dir, ForceMode.Acceleration);

    }
    float I = 0;

    public Vector3 HorizontalPID()
    {
        return Vector3.zero;
    }
    float _lastValidalt;
    public Vector3 VerticalPID()
    {
        float MeasuredAlt = _lastValidalt;
        RaycastHit hit;
        float altitudeRate = _rb.linearVelocity.y;
        Vector3 dir = Vector3.up;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, groundMask))
        {
            
            MeasuredAlt = hit.distance;
            altitudeRate = -Vector3.Dot(_rb.linearVelocity, hit.normal);
            _lastValidalt = MeasuredAlt;
        }
        float dt = Time.fixedDeltaTime;
        float err = TargetAlt - MeasuredAlt;

        I = Mathf.Clamp(I + err * dt, -MaxI, MaxI);

        float a_y_unclamped = Kp * err + Ki * I - Kd * altitudeRate;
        Vector3 a_y_clamped = Mathf.Clamp(a_y_unclamped, -MaxAccel, MaxAccel)*dir;
        return a_y_clamped;

    }

    // ===== IGoalMotor =====
    public void MoveTo(Vector3 dest, float stopDistance = 0.25f, float? altitude = null)
    {
        _goal = dest;
    }

    public void Follow(Transform target, float updateDist = 0.75f, float updateSeconds = 0.2f, float? altitudeOffset = null)
    {
        
    }

    public void Stop()
    {
        
    }

    public bool CanPathTo(Vector3 dest) => true; // no ground pathfinding for hover

    // ===== IMotor compatibility =====
    public void SetVelocity(Vector3 v)
    {
        
    }

    public void SetAltitude(float? height, Transform relTo)
    {
       
    }


    
}
