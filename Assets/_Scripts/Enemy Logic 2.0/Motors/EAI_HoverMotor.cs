
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(50)]
public class HoverMotor : MonoBehaviour, IGoalMotor, IKnockbackable
{
    [Header("Motion")]
    public float maxSpeed = 8f;
    public float MaxAccel;
    public float accel = 25f;              
    public float arriveRadius = 0.5f;       

    [Header("Altitude Hold")]
    public float defaultAltitude = 3.0f;    
    public LayerMask groundMask = ~0;

    [Header("PID-Vert")]
    public float MaxI_V = 1;
    public float Kp_V;
    public float Ki_V;
    public float Kd_V;
    public float TargetAlt;

    [Header("PID-Hor")]
    public float MaxI_H = 1;
    public float Kp_H = 1;
    public float Ki_H = 0;
    public float Kd_H = -1;

    [Header("Avoidance")]
    public LayerMask obstacleMask = ~0;    // set this to walls/props (NOT ground)
    public float agentRadius = 0.6f;
    public float lookAheadBase = 3f;
    public float lookAheadPerSpeed = 0.5f;
    public float sideAngle = 35f;
    public float sideWhiskerScale = 0.7f;
    public float avoidStrength = 25f;      // accel added when close
    [Range(0f, 1f)] public float slide = 0.7f; // 0=bounce off, 1=slide along
    public int WhiskerAmount;

    [Header("Unstick")]
    public float depenetrationBoost = 12f; // tiny push off wall
    public float unstuckAccel = 18f;       // slide along wall when pinned
    public float stuckSpeed = 0.25f;       // “I’m basically not moving”
    public float stuckTime = 0.25f;        // how long before we kick

    Vector3 _contactNormal;
    bool _hasContact;
    float _stuckT;

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

    public Vector3 Velocity => _rb.linearVelocity;

    Rigidbody _rb;
    float _CurrentAlt;
    Vector3 _goal;
    

    void Awake()
    {
        _rb = GetComponent<Rigidbody>(); // optional
        //_rb.maxLinearVelocity = maxSpeed;
        _goal = transform.position;
    }

    private void Update()
    {
        //_rb.maxLinearVelocity = MaxSpeed;
    }

    void FixedUpdate()
    {
        if (!Enabled) return;

        Vector3 ay = VerticalPID();
        Vector3 Aflat = HorizontalPD();
        Vector3 avoid = ObstacleAvoidance();

        Vector3 acc = ay + Aflat + avoid;
        acc = Vector3.ClampMagnitude(acc, MaxAccel);
        acc = HasContact(acc);
        acc = LimitAccelBySpeed(acc, Time.fixedDeltaTime);
        _rb.AddForce(acc, ForceMode.Acceleration);

    }

    float I_v = 0;
    float I_h = 0;

    public Vector3 HorizontalPD()
    {
        
        Vector3 to = _goal - transform.position;
        to.y = 0f;
        float dist = to.magnitude; if (dist < 1e-4f) return Vector3.zero;
        Vector3 n = to / dist;
        Vector3 v = _rb.linearVelocity; v.y = 0f;

        float vAlong = Vector3.Dot(v, n);
        Vector3 vSide = v - vAlong * n;

        float aAlong = Kp_H * dist - Mathf.Abs(Kd_H) * vAlong; // Kd_H > 0
        Vector3 a = aAlong * n - (Mathf.Abs(Kd_H) * vSide);    // damp sideways drift too
        return Vector3.ClampMagnitude(a, MaxAccel);
    }

    
    float _lastValidalt;
    float groundY;
    public Vector3 VerticalPID()
    {
        float Target = TargetAlt;
        float MeasuredAlt = transform.position.y;
        RaycastHit hit;
        float altitudeRate = _rb.linearVelocity.y;
        Vector3 dir = Vector3.up;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, groundMask) )
        {
            if(hit.point.y > Target)
            {
                Target = hit.point.y + defaultAltitude;
            }
            if (RelToGround) 
            {
                
                MeasuredAlt = hit.distance;
                groundY = hit.point.y;

                altitudeRate = -Vector3.Dot(_rb.linearVelocity, hit.normal);
                _lastValidalt = MeasuredAlt;
            }
            
        }
        float dt = Time.fixedDeltaTime;
        float err = Target - MeasuredAlt;

        I_v = Mathf.Clamp(I_v + err * dt, -MaxI_V, MaxI_V);

        float a_y_unclamped = Kp_V * err + Ki_V * I_v - Kd_V * altitudeRate;
        Vector3 a_y_clamped = Mathf.Clamp(a_y_unclamped, -MaxAccel, MaxAccel)*dir;
        return a_y_clamped;

    }

    // ===== IGoalMotor =====
    public void MoveTo(Vector3 dest, float stopDistance = 0.25f, float? altitude = null)
    {
        _goal = dest;
        if (altitude != null) SetAltitude((float)altitude);
    }


    public void Stop()
    {
        _goal = transform.position;
    }

    public bool CanPathTo(Vector3 dest) => true; // no ground pathfinding for hover

    // ===== IMotor compatibility =====
    public void SetVelocity(Vector3 v)
    {
        _rb.linearVelocity = v;
    }

    bool RelToGround;

    public void SetAltitude(float height, Transform relTo = null)
    {
        if(relTo != null)
        {
            RelToGround = false;
            _goal.y = relTo.position.y + height;
        }
        else
        {
            RelToGround = true;
            _goal.y = height;
        }
        TargetAlt = _goal.y;
    }
    Vector3 _ObstacleDir;
    Vector3 ObstacleAvoidance()
    {
        Vector3 vel = _rb ? _rb.linearVelocity : Vector3.zero;
        float speed = vel.magnitude;

        // forward guess: use velocity if moving, else toward goal
        Vector3 toGoal = _goal - transform.position;
        Vector3 fwd = (speed > 0.1f ? vel : toGoal).normalized;
        if (fwd.sqrMagnitude < 1e-6f) return Vector3.zero;

        float stopping = (speed * speed) / Mathf.Max(0.01f, 2f * MaxAccel);
        float look = Mathf.Clamp(lookAheadBase + speed * lookAheadPerSpeed + stopping, 1f, 20f);
        Vector3 origin = transform.position + Vector3.up * (agentRadius * 0.5f); 

        Vector3 sum = Vector3.zero;
        float sumW = 0;

        int n = Mathf.Max(1, WhiskerAmount);
        //Horizontal Whiskers
        for (int i = -n; i <= n; i++)
        {
            float nt = (float)i / n;
            float ang = nt * sideAngle;
            Vector3 dir = Quaternion.Euler(0, ang, 0f) * fwd;

            float len = Mathf.Lerp(look, look * sideWhiskerScale, Mathf.Abs(nt));
            //Forward bias
            float w = Mathf.Cos(Mathf.Deg2Rad * Mathf.Abs(ang));

            Vector3 steer = CastSteer(origin, dir, len, 1);
            if (steer.sqrMagnitude > 0f)
            {
                sum += steer * w;
                sumW += w;
            }
        }

        //Vertical Whiskers
        for (int i = -n; i <= n; i++)
        {
            float nt = (float)i / n;                       // -1..+1
            float ang = nt * sideAngle;                    
            Vector3 dir = Quaternion.Euler(ang, 0f, 0f) * fwd;

            float len = Mathf.Lerp(look, look * sideWhiskerScale, Mathf.Abs(nt));

            // bias toward near-flat; still allow strong up/down if needed
            float w = Mathf.Cos(Mathf.Deg2Rad * (Mathf.Abs(ang) * 0.75f));

            Vector3 steer = CastSteer(origin, dir, len, 1f);
            if (steer.sqrMagnitude > 0f)
            {
                sum += steer * w;
                sumW += w;
            }
        }

        Vector3 blended = (sumW > 0f) ? (sum / Mathf.Max(1e-6f, sumW)) : Vector3.zero;

        //Pick Best Option
        var best = blended;

        Debug.DrawRay(origin, best, UnityEngine.Color.cyan);

        _ObstacleDir = Vector3.ClampMagnitude(best * avoidStrength, MaxAccel);
        return Vector3.ClampMagnitude(best * avoidStrength, MaxAccel);
    }

    Vector3 CastSteer(Vector3 origin, Vector3 dir, float dist, float weight)
    {
        Debug.DrawRay(origin, dist * dir, UnityEngine.Color.red);
        RaycastHit hit;
        if (Physics.SphereCast(origin, agentRadius, dir, out hit, dist, obstacleMask, QueryTriggerInteraction.Ignore))
        {
            float t = 1f - (hit.distance / dist);   // closer = stronger
            Vector3 away = hit.normal;              // push off the surface
            Vector3 along = Vector3.ProjectOnPlane(dir, hit.normal).normalized; // slide
            Vector3 steerDir = Vector3.Lerp(away, along, slide).normalized;
            
            return steerDir * (t * weight);
        }
        return Vector3.zero;
    }

    void OnCollisionStay(Collision c)
    {
        Vector3 n = Vector3.zero; int count = 0;
        foreach (var cp in c.contacts)
            if (Vector3.Dot(cp.normal, Vector3.up) < 0.7f) { n += cp.normal; count++; }

        if (count > 0)
        {
            _contactNormal = n.normalized;
            _hasContact = true;
        }
        else
        {
            _hasContact = false;  
        }
    }


    void OnCollisionExit(Collision c)
    {
        _hasContact = false;
    }

    public Vector3 HasContact(Vector3 acc)
    {
        // slide on walls, never push into them
        if (_hasContact)
        {
            // 1) remove any acceleration into the wall
            acc = Vector3.ProjectOnPlane(acc, _contactNormal);

            // 2) add a tiny outward bias to help depenetration solve
            acc += _contactNormal * depenetrationBoost;

            // 3) kill current velocity into the wall (prevents “glue”)
            Vector3 v = _rb.linearVelocity; // or _rb.velocity if that’s your API
            float vin = Vector3.Dot(v, -_contactNormal);
            if (vin > 0f)
                v += _contactNormal * vin;   // remove inward component
            _rb.linearVelocity = v;

            // 4) unstuck kick: if we’re barely moving for a bit, push along tangent toward goal
            if (v.magnitude < stuckSpeed) _stuckT += Time.fixedDeltaTime;
            else _stuckT = 0f;

            if (_stuckT > stuckTime)
            {
                Vector3 towardGoal = _goal - transform.position;
                Vector3 tangent = Vector3.ProjectOnPlane(towardGoal, _contactNormal).normalized;
                if (tangent.sqrMagnitude < 1e-6f) tangent = Vector3.Cross(_contactNormal, Vector3.up).normalized;

                acc += tangent * unstuckAccel;
                _stuckT = 0.1f; // don’t spam every frame
            }
        }
        return acc;

    }

    // call this right before AddForce
    Vector3 LimitAccelBySpeed(Vector3 acc, float dt)
    {
        Vector3 v = _rb.linearVelocity;                // or _rb.velocity
        float vm = v.magnitude;
        if (vm < 1e-4f) return Vector3.ClampMagnitude(acc, MaxAccel);

        Vector3 vDir = v / vm;

        // split accel into tangential (along v) and normal (turning) parts
        float a_t = Vector3.Dot(acc, vDir);
        Vector3 a_n = acc - a_t * vDir;

        // don’t let next-step speed exceed max
        float allow = (maxSpeed - vm) / Mathf.Max(1e-6f, dt);   // allowed tangential accel
        if (vm >= maxSpeed) a_t = Mathf.Min(a_t, 0f);           // only brake along v when over max
        else a_t = Mathf.Min(a_t, allow);        // forward thrust capped

        Vector3 shaped = a_n + a_t * vDir;
        return Vector3.ClampMagnitude(shaped, MaxAccel);
    }


    void OnDrawGizmos() // runs in edit & play mode (Editor only)
    {
        //Y Target
        var pos = (new Vector3(transform.position.x, TargetAlt, transform.position.z));

        UnityEngine.Color old = Gizmos.color;
        Gizmos.color = UnityEngine.Color.cyan;

        Gizmos.DrawSphere(pos, 0.4f);

        Gizmos.color = old;


        // Goal
        pos = _goal;
        old = Gizmos.color;
        Gizmos.color = UnityEngine.Color.red;

        Gizmos.DrawSphere(pos, 0.4f);
    }

    public void EnablePhysics(bool x)
    {
        return;
    }

    public void GetKnockedBack(Vector3 force, ForceMode forceMode = ForceMode.Force)
    {
        _rb.AddForce(force, forceMode);
    }

    public void GetKnockedBack(Vector3 force, Vector3 point)
    {
        _rb.AddForceAtPosition(force, point);
    }

    public void SetPosition(Vector3 dest)
    {
        _rb.MovePosition(dest);
    }

}
