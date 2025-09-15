using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class NavMeshMotor : MonoBehaviour, IGoalMotor, IKnockbackable
{
    [Header("Repathing")]
    public float repathMoveThreshold = 0.3f;


    [Header("Rotation")]
    public bool controlRotation = true;
    public float faceTurnSpeed = 720f;

    private NavMeshAgent agent;
    private Vector3 lastIssuedDest;
    private float desiredStopDistance;
    [Range(0.001f, 0.1f)]public float StillThreshold = 0.05f;
    
    public float MaxSpeed { get => agent.speed; set => agent.speed = value; }
    public bool Enabled { get => agent.enabled; set => agent.enabled = value; }
    public bool ReachedGoal => !agent.pathPending && agent.remainingDistance <= Mathf.Max(agent.stoppingDistance, desiredStopDistance);
    public Vector3 Destination => agent.hasPath ? agent.destination : lastIssuedDest;

    public Vector3 Velocity => getCorrectVelocity();

    public Vector3 getCorrectVelocity()
    {
        if (agent.isActiveAndEnabled) return agent.velocity;
        else return rb.linearVelocity;
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = controlRotation;
        agent.autoBraking = true;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    }

    Rigidbody rb;
    private void Start()
    {
        agent.updateRotation = controlRotation;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public void MoveTo(Vector3 dest, float stopDistance = 0.25f, float? altitude = null)
    {
        if (CheckDestination(dest)) 
        {
            if (agent.SetDestination(dest))
            {
                agent.isStopped = false;
                lastIssuedDest = transform.position;
            }
            
        }
    }


    public void Stop()
    {
        agent.ResetPath();
        lastIssuedDest = transform.position;
    }

    public bool CanPathTo(Vector3 dest)
    {
        return true;
    }

    // velocity based IMotor compatibility
    public void SetVelocity(Vector3 v)
    {
        if (!agent.enabled) return;
        agent.isStopped = true;
    }

    
    public void SetAltitude(float height, Transform relTo) {}

    //Makes Path refresh a bit more chill
    bool CheckDestination(Vector3 p)
    {
        return (Vector3.Distance(p, lastIssuedDest) > repathMoveThreshold || agent.isPathStale) && agent.isActiveAndEnabled;
    }

    public void EnablePhysics(bool x)
    {
        
        rb.useGravity = x;
        rb.isKinematic = !x;
        rb.freezeRotation = x;
        if (!x)
        {
            agent.Warp(transform.position);
        }
        agent.enabled = !x;
        MoveTo(lastIssuedDest);
    }

    public void GetKnockedBack(Vector3 force)
    {
        StartCoroutine(ApplyKnockBack(force));
    }

    public void GetKnockedBack(Vector3 force, Vector3 point)
    {
        StartCoroutine(ApplyKnockBack(force, point));
    }

    private IEnumerator ApplyKnockBack(Vector3 force)
    {
        Debug.Log("KB!");
        yield return null;
        EnablePhysics(true);
        rb.AddForce(force);

        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => rb.linearVelocity.magnitude < StillThreshold);
        EnablePhysics(false);   
    }

    private IEnumerator ApplyKnockBack(Vector3 force, Vector3 point)
    {
        Debug.Log("KB point!");
        yield return null;
        EnablePhysics(true);
        rb.AddForceAtPosition(force, point);

        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => rb.linearVelocity.magnitude < StillThreshold);
        
        EnablePhysics(false);
    }

    public void SetPosition(Vector3 dest)
    {
        agent.Warp(dest);
    }
}
