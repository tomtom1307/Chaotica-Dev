
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(EAI_Blackboard))]
public class EAI_Perception : MonoBehaviour, IPerception
{
    [SerializeField]
    AnimationCurve distanceCurve =
    AnimationCurve.EaseInOut(0f, 1f, 1f, 0f); 

    [Header("Targeting")]
    public string playerTag = "Player";
    public float refreshHz = 10f;

    [Header("Detection Gate")]
    public float maxDetectRange = 30f;   // hard range
    public float fovDegrees = 120f;      // vision cone
    public LayerMask losMask;            // walls/ground

    [Header("Detection")]
    public float DetectionMeter = 0;
    public float DetectionSpeed = 1;
    public float DetectionDecay = 1.5f;

    EAI_Blackboard bb;
    float t, lastSeenTime;

    void Awake() { bb = GetComponent<EAI_Blackboard>(); }

    void Update()
    {
        t += Time.deltaTime;
        if (t < 1f / refreshHz) return;
        t = 0f;

        // Get target if missing
        if (!bb.target)
        {
            var p = GameObject.FindGameObjectWithTag(playerTag);
            if (p) bb.target = p.transform;
        }
        if (!bb.target) return;

        Vector3 to = bb.target.position - transform.position;
        float dist = to.magnitude;
        bb.distanceToTarget = dist;
        float ang = 0;
        // Range gate
        bool inRange = dist <= maxDetectRange;

        // FOV gate
        bool inFOV = true;
        if (fovDegrees < 360f)
        {
            Vector3 fwd = transform.forward;
            ang = Vector3.Angle(fwd, to);
            inFOV = ang <= fovDegrees * 0.5f;
        }

        // LOS gate
        bool hasLOS = false;
        if (inRange && inFOV)
        {
            hasLOS = HasLOS(transform.position, bb.target.position, 0.25f);
            bb.hasLOS = hasLOS;
        }

        if (hasLOS)
        {

            bb.lastKnownTargetPos = bb.target.position;
            lastSeenTime = Time.time;



            float distW = DistanceWeight(dist);
            float angW = AngleWeight(ang);
            DetectionMeter += DetectionSpeed * Time.deltaTime;
        }
        else DetectionMeter -= DetectionDecay * Time.deltaTime;
        DetectionMeter = Mathf.Clamp01(DetectionMeter);
        bb.DetectionMeter = DetectionMeter;
        bb.LastSeenPlayerTime = Time.time - lastSeenTime; 
    }

    

    public Transform PrimaryTarget => bb.target;

    public float DistanceToTarget => bb.distanceToTarget;

    public bool HasLOS(Vector3 from, Vector3 to, float radius)
    {
        Vector3 dir = to - from; float d = dir.magnitude;
        if (d < 0.05f) return true;
        return !Physics.SphereCast(from, radius, dir.normalized, out _, d, losMask, QueryTriggerInteraction.Ignore);
    }


    private float AngleWeight(float ang)
    {
        if (fovDegrees >= 360f) return 1f;
        float half = fovDegrees * 0.5f;
        float w = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(half, 0f, Mathf.Abs(ang)));
        w = Mathf.Max(w, 0.1f);
        return w;
    }

    private float DistanceWeight(float dist)
    {
        float x = Mathf.Clamp01(dist / maxDetectRange);
        return Mathf.Clamp01(distanceCurve.Evaluate(x));
    }

    public void SetDetectionMeter(float SetVal)
    {
        DetectionMeter = Mathf.Clamp01(SetVal);
    }


    
}

