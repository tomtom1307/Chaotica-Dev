using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMotor : MonoBehaviour, IGoalMotor
{
    [Header("Repathing")]
    public float minRepathInterval = 0.15f;
    public float repathMoveThreshold = 0.6f;

    [Header("Sampling")]
    public float targetSampleRadius = 2f;

    [Header("Rotation")]
    public bool controlRotation = true;
    public float faceTurnSpeed = 720f;

    private NavMeshAgent agent;
    private Transform followTarget;
    private float nextRepathAt;
    private Vector3 lastIssuedDest;
    private float desiredStopDistance;

    public MotorCaps Caps => MotorCaps.Destination | MotorCaps.Pathfinding;
    public float MaxSpeed { get => agent.speed; set => agent.speed = value; }
    public bool Enabled { get => agent.enabled; set => agent.enabled = value; }
    public bool ReachedGoal => !agent.pathPending && agent.remainingDistance <= Mathf.Max(agent.stoppingDistance, desiredStopDistance);
    public Vector3 Destination => agent.hasPath ? agent.destination : lastIssuedDest;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = controlRotation;
        agent.autoBraking = true;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    }

    void Update()
    {
        if (followTarget) TryRepathToFollow();
        if (controlRotation && agent.velocity.sqrMagnitude > 0.01f)
        {
            var fwd = agent.velocity; fwd.y = 0;
            if (fwd.sqrMagnitude > 0.001f)
            {
                var tgt = Quaternion.LookRotation(fwd.normalized, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, tgt, faceTurnSpeed * Time.deltaTime);
            }
        }
    }

    public void MoveTo(Vector3 dest, float stopDistance = 0.25f, float? altitude = null)
    {
        followTarget = null;
        desiredStopDistance = Mathf.Max(0, stopDistance);
        if (!NavMesh.SamplePosition(dest, out var hit, targetSampleRadius, agent.areaMask))
            hit.position = dest;
        IssueDestination(hit.position);
    }

    public void Follow(Transform target, float updateDist = 0.75f, float updateSeconds = 0.2f, float? altitudeOffset = null)
    {
        followTarget = target;
        repathMoveThreshold = Mathf.Max(0.05f, updateDist);
        minRepathInterval = Mathf.Max(0.05f, updateSeconds);
        desiredStopDistance = agent.stoppingDistance;
        nextRepathAt = 0f;
        TryRepathToFollow();
    }

    public void Stop()
    {
        followTarget = null;
        agent.ResetPath();
        lastIssuedDest = transform.position;
    }

    public bool CanPathTo(Vector3 dest)
    {
        if (!NavMesh.SamplePosition(dest, out var hit, targetSampleRadius, agent.areaMask)) return false;
        var path = new NavMeshPath();
        return agent.enabled && NavMesh.CalculatePath(transform.position, hit.position, agent.areaMask, path) && path.status == NavMeshPathStatus.PathComplete;
    }

    // velocity based IMotor compatibility
    public void SetVelocity(Vector3 v)
    {
        if (!agent.enabled) return;
        if ((followTarget == null) && (!agent.hasPath || ReachedGoal) && v.sqrMagnitude > 0.001f)
            MoveTo(transform.position + v.normalized * 2f, 0f);
    }
    public void SetAltitude(float? height, Transform relTo) {}

    void TryRepathToFollow()
    {
        if (!followTarget || !agent.enabled) return;
        if (Time.time < nextRepathAt) return;

        var pos = followTarget.position;
        if (!NavMesh.SamplePosition(pos, out var hit, targetSampleRadius, agent.areaMask)) hit.position = pos;

        if ((hit.position - lastIssuedDest).sqrMagnitude >= repathMoveThreshold * repathMoveThreshold
            || agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            IssueDestination(hit.position);
        }
    }

    void IssueDestination(Vector3 p)
    {
        nextRepathAt = Time.time + minRepathInterval;
        lastIssuedDest = p;
        agent.stoppingDistance = desiredStopDistance;
        agent.isStopped = false;
        agent.SetDestination(p);
    }
}
