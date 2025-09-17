using UnityEngine;


//Holds all Relevant information from perception
public class EAI_Blackboard : MonoBehaviour
{
    [Header("Standard")]
    public string CurrentState;
    public Transform target;

    [Header("Detection")]
    public float DetectionMeter;
    public float LastSeenPlayerTime;
    public Vector3 lastKnownTargetPos;
    public float distanceToTarget;
    public bool hasLOS;
    public bool isInRange;

    [Header("Combat")]
    public AttackState attack_State;
    public bool AttackAvailable;

    [Header("Searching")]
    public Vector3 POI;
    public bool Search;
    public bool InvestigateSound;

    [Header("Aggro")]
    public bool isAggro;                 // derived flag
    public bool tookDamageRecently;      // set by health/hit events

    public enum AttackState
    {
        ready,
        attacking,
        cooldown
    }


}

