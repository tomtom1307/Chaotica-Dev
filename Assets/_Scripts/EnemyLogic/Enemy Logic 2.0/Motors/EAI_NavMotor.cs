using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMotor : MonoBehaviour, IMotor
{
    [Header("Update & Lookahead")]
    public float updateRate = 0.3f;   // seconds between destination nudges
    public float lookahead = 2.5f;   // meters ahead per nudge
    public float sampleRadius = 1.0f; // snap-to-walkable radius if goal is off the mesh

    [Header("Fallbacks")]
    public bool useSteeringTargetFallback = true; // follow next corner if goal invalid

    NavMeshAgent agent;
    float nextUpdate;
    Vector3 lastGoal;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // Quality avoidance tweaks (optional)
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    }

    // IMotor
    public float MaxSpeed { get => agent.speed; set => agent.speed = value; }
    public bool Enabled { get => agent.enabled; set => agent.enabled = value; }

    public void SetVelocity(Vector3 v)
    {
        if (!agent.enabled) return;

        if (Time.time < nextUpdate) return;
        nextUpdate = Time.time + updateRate;

        // Build a small goal ahead in the intended direction
        v.y = 0f;
        Vector3 dir = v.sqrMagnitude > 0.0001f ? v.normalized : Vector3.zero;
        Vector3 rawGoal = transform.position + dir * lookahead;

        // Snap goal to nearest walkable if needed
        Vector3 goal = rawGoal;
        if (!SampleToNavMesh(rawGoal, out goal))
        {
            // Try a shorter lookahead
            if (!SampleToNavMesh(transform.position + dir * (lookahead * 0.5f), out goal))
            {
                // As a last resort, steer toward next corner if we have a path
                if (useSteeringTargetFallback && agent.hasPath)
                {
                    goal = agent.steeringTarget;
                }
                else
                {
                    // Give up this tick
                    return;
                }
            }
        }

        lastGoal = goal;
        agent.SetDestination(goal);
    }

    public void SetAltitude(float? height, Transform relTo) { /* ground ignores */ }

    bool SampleToNavMesh(Vector3 point, out Vector3 snapped)
    {
        if (NavMesh.SamplePosition(point, out var hit, sampleRadius, agent.areaMask))
        {
            snapped = hit.position;
            return true;
        }
        snapped = Vector3.zero;
        return false;
    }

    // Optional helper if you ever want to set a point goal directly
    public void SetPointGoal(Vector3 worldPoint)
    {
        if (!agent.enabled) return;
        if (SampleToNavMesh(worldPoint, out var p)) agent.SetDestination(p);
    }
}
