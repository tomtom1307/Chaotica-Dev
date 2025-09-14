using UnityEditor.Experimental.GraphView;
using UnityEngine;


//Holds all references
[RequireComponent(typeof(EAI_Blackboard))]
[RequireComponent(typeof(Aimer))]
[RequireComponent(typeof(EAI_Perception))]
[RequireComponent(typeof(EAI_StateMachine))]
[RequireComponent(typeof(EnemyAbilityRunner))]
[RequireComponent (typeof(DamagableEnemy))]
public class EnemyContext : MonoBehaviour
{
    public EnemyTuningSO cfg;
    public Transform muzzle;
    public bool DebugStates;
    [HideInInspector] public EAI_Blackboard bb;
    [HideInInspector] public IGoalMotor motor;
    [HideInInspector] public Aimer aimer;
    [HideInInspector] public IAbilityRunner abilities;
    [HideInInspector] public EAI_Perception sense;
    [HideInInspector] public float nextBurstAt;
    [HideInInspector] public EAI_StateMachine stateMachine;

    void Awake()
    {
        bb = GetComponent<EAI_Blackboard>();
        motor = GetComponent<IGoalMotor>();
        aimer = GetComponent<Aimer>();
        abilities = GetComponent<IAbilityRunner>();
        sense = GetComponent<EAI_Perception>();
        stateMachine = GetComponent<EAI_StateMachine>();
    }

    private void Update()
    {
        bb.isInRange = bb.distanceToTarget <= cfg.preferredMax && bb.distanceToTarget >= cfg.preferredMin;

    }


    public void PostTrigger(EAI_TriggerID triggerID, IEAI_TriggerPayload? triggerPayload)
    {

    }

    public void HeardNoise(Vector3 pos)
    {
        Debug.Log("Heard Somet!");
        bb.InvestigateSound = true;
        Search(pos);
    }

    public void Search()
    {
        Search(bb.lastKnownTargetPos);
    }

    public void Search(Vector3 pos)
    {
        bb.POI = pos;
        bb.Search = true;
        bb.DetectionMeter = 0.4f;
    }

    public void Agro(bool x)
    {
        Debug.Log("Enemy Aggro!");
        if (x)
        {
            bb.POI = bb.target.position;
            bb.lastKnownTargetPos = bb.target.position;
            bb.LastSeenPlayerTime = 0;
            sense.SetDetectionMeter(1);
        }
        else
        {
            bb.LastSeenPlayerTime = 100;
            sense.SetDetectionMeter(0);
        }
    }

}
