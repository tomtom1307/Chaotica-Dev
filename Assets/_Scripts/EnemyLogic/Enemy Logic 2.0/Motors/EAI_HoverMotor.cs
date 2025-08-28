using UnityEngine;

[DefaultExecutionOrder(50)]
public class HoverMotor3D : MonoBehaviour, IGoalMotor
{
    [Header("Motion")]
    public float maxSpeed = 8f;
    public float accel = 25f;               // m/s� lateral accel
    public float turnRate = 720f;           // deg/s facing toward velocity
    public float arriveRadius = 0.5f;       // goal reached within this (horizontal)
    public float slowRadius = 3.0f;         // start slowing within this (horizontal)

    [Header("Altitude Hold")]
    public float defaultAltitude = 3.0f;    // meters AGL if you use ground
    public bool useGroundRef = false;      // if true, altitude is relative to ground hit
    public LayerMask groundMask = ~0;       // used when useGroundRef is true
    public float altKp = 4.0f;              // PD gains
    public float altKd = 1.0f;
    public float altTolerance = 0.4f;       // considered �at alt� within this

    [Header("Obstacle Avoidance")]
    public LayerMask obstacleMask = ~0;
    public float avoidProbe = 3.0f;         // forward probe distance
    public float sideProbe = 2.0f;          // side probes
    public float avoidWeight = 1.5f;        // steering weight

    [Header("Misc")]
    public bool controlRotation = true;

    // IGoalMotor caps
    public MotorCaps Caps => MotorCaps.Destination | MotorCaps.Hover3D;

    // IMotor
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public bool Enabled { get => enabled; set => enabled = value; }

    // IGoalMotor state
    public bool ReachedGoal
    {
        get
        {
            var flatDist = Vector3.Distance(Flat(transform.position), Flat(_goal));
            bool posOK = flatDist <= Mathf.Max(_stopDist, arriveRadius);
            bool altOK = !_wantAlt.HasValue || Mathf.Abs(AltitudeError()) <= altTolerance;
            return posOK && altOK && !_following;
        }
    }
    public Vector3 Destination => _goal;

    Rigidbody _rb;
    Transform _follow;
    Vector3 _goal;
    float _stopDist = 0.3f;
    float? _wantAlt;           // world Y or AGL depending on useGroundRef
    float _nextFollowUpdate;
    float _followDist = 0.75f;
    float _followInterval = 0.2f;

    Vector3 _vel;              // kinematic velocity if no RB

    void Awake()
    {
        _rb = GetComponent<Rigidbody>(); // optional
        _goal = transform.position;
        _wantAlt = useGroundRef ? defaultAltitude : (float?)null;
    }

    void Update()
    {
        if (!Enabled) return;

        // Update follow goal
        if (_following && Time.time >= _nextFollowUpdate && _follow)
        {
            _goal = _follow.position;
            _nextFollowUpdate = Time.time + _followInterval;
        }

        // Compute desired horizontal velocity (Arrive)
        Vector3 to = Flat(_goal) - Flat(transform.position);
        float dist = to.magnitude;
        Vector3 dir = dist > 0.001f ? to / dist : Vector3.zero;

        float targetSpeed = maxSpeed;
        if (dist < slowRadius) targetSpeed = Mathf.Lerp(0f, maxSpeed, Mathf.InverseLerp(0f, slowRadius, dist));
        if (dist <= _stopDist) targetSpeed = 0f;

        Vector3 desiredHorizVel = dir * targetSpeed;

        // Obstacle avoidance (simple probes)
        Vector3 avoid = Vector3.zero;
        if (desiredHorizVel.sqrMagnitude > 0.0001f)
        {
            Vector3 fwd = desiredHorizVel.normalized;
            Vector3 origin = transform.position;
            if (Physics.SphereCast(origin, 0.5f, fwd, out var hit, avoidProbe, obstacleMask, QueryTriggerInteraction.Ignore))
                avoid += Vector3.ProjectOnPlane((origin - hit.point), Vector3.up).normalized;

            Vector3 left = Quaternion.Euler(0, -35, 0) * fwd;
            if (Physics.SphereCast(origin, 0.4f, left, out hit, sideProbe, obstacleMask, QueryTriggerInteraction.Ignore))
                avoid += Quaternion.Euler(0, 90, 0) * fwd;

            Vector3 right = Quaternion.Euler(0, +35, 0) * fwd;
            if (Physics.SphereCast(origin, 0.4f, right, out hit, sideProbe, obstacleMask, QueryTriggerInteraction.Ignore))
                avoid += Quaternion.Euler(0, -90, 0) * fwd;
        }

        if (avoid.sqrMagnitude > 0.001f)
            desiredHorizVel = Vector3.ClampMagnitude(desiredHorizVel + avoid * avoidWeight * maxSpeed, maxSpeed);

        // Altitude hold (PD)
        float desiredYVel = 0f;
        if (_wantAlt.HasValue)
        {
            float err = AltitudeError();                     // desired - current
            float vy = _rb ? _rb.linearVelocity.y : _vel.y;
            desiredYVel = Mathf.Clamp(altKp * err - altKd * vy, -maxSpeed, maxSpeed);
        }

        Vector3 desiredVel = new Vector3(desiredHorizVel.x, desiredYVel, desiredHorizVel.z);

        // Apply movement
        if (_rb)
        {
            // Smooth toward desired velocity with accel limit
            Vector3 cur = _rb.linearVelocity;
            Vector3 newVel = Vector3.MoveTowards(cur, desiredVel, accel * Time.deltaTime);
            _rb.linearVelocity = newVel;
        }
        else
        {
            _vel = Vector3.MoveTowards(_vel, desiredVel, accel * Time.deltaTime);
            transform.position += _vel * Time.deltaTime;
        }

        // Face velocity
        if (controlRotation)
        {
            Vector3 face = new Vector3((_rb ? _rb.linearVelocity.x : _vel.x), 0, (_rb ? _rb.linearVelocity.z : _vel.z));
            if (face.sqrMagnitude > 0.01f)
            {
                var t = Quaternion.LookRotation(face.normalized, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, t, turnRate * Time.deltaTime);
            }
        }
    }

    // ===== IGoalMotor =====
    public void MoveTo(Vector3 dest, float stopDistance = 0.25f, float? altitude = null)
    {
        _following = false; _follow = null;
        _goal = dest; _stopDist = Mathf.Max(0f, stopDistance);
        if (altitude.HasValue) _wantAlt = useGroundRef ? altitude.Value : (dest.y + altitude.Value); // if AGL, pass meters; else treat as offset
    }

    public void Follow(Transform target, float updateDist = 0.75f, float updateSeconds = 0.2f, float? altitudeOffset = null)
    {
        _following = true; _follow = target;
        _followDist = Mathf.Max(0.05f, updateDist);
        _followInterval = Mathf.Max(0.05f, updateSeconds);
        _nextFollowUpdate = 0f;
        // Hold a relative altitude if provided
        if (altitudeOffset.HasValue)
            _wantAlt = useGroundRef ? altitudeOffset.Value : (target.position.y + altitudeOffset.Value);
    }

    public void Stop()
    {
        _following = false; _follow = null;
        _goal = transform.position;
        _stopDist = arriveRadius;
        if (_rb) _rb.linearVelocity = Vector3.zero; else _vel = Vector3.zero;
    }

    public bool CanPathTo(Vector3 dest) => true; // no ground pathfinding for hover

    // ===== IMotor compatibility =====
    public void SetVelocity(Vector3 v)
    {
        // Direct nudge; clears goals
        _following = false; _follow = null; _goal = transform.position + v;
        if (_rb) _rb.linearVelocity = v; else _vel = v;
    }

    public void SetAltitude(float? height, Transform relTo)
    {
        if (!height.HasValue) { _wantAlt = null; return; }
        if (useGroundRef)
        {
            // Treat as meters above ground
            _wantAlt = height.Value;
        }
        else
        {
            float baseY = relTo ? relTo.position.y : 0f;
            _wantAlt = baseY + height.Value;
        }
    }

    // ===== Helpers =====
    bool _following;
    float AltitudeError()
    {
        float curY = transform.position.y;
        float desiredY = _wantAlt ?? curY;

        if (useGroundRef)
        {
            // Desired = groundY + wantAlt
            if (Physics.Raycast(transform.position + Vector3.up * 5f, Vector3.down, out var hit, 50f, groundMask, QueryTriggerInteraction.Ignore))
                desiredY = hit.point.y + (_wantAlt ?? defaultAltitude);
        }
        return desiredY - curY;
    }

    static Vector3 Flat(Vector3 v) => new Vector3(v.x, 0f, v.z);

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_goal, 0.25f);
        if (useGroundRef && _wantAlt.HasValue)
        {
            float desired = transform.position.y + AltitudeError();
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, desired, transform.position.z));
        }
    }
#endif
}
