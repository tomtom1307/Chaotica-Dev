using UnityEditor.Experimental.GraphView;
using UnityEngine;


//Holds all references
public class EnemyContext : MonoBehaviour
{
    public EnemyTuningSO cfg;
    public Transform muzzle;
    public bool DebugStates;
    [HideInInspector] public Blackboard bb;
    [HideInInspector] public IGoalMotor motor;
    [HideInInspector] public Aimer aimer;
    [HideInInspector] public IAbilityRunner abilities;
    [HideInInspector] public IPerception sense;
    [HideInInspector] public float nextBurstAt;
    EAI_StateMachine EAI_StateMachine;

    void Awake()
    {
        bb = GetComponent<Blackboard>();
        motor = GetComponent<IGoalMotor>();
        aimer = GetComponent<Aimer>();
        abilities = GetComponent<IAbilityRunner>();
        sense = GetComponent<IPerception>();
    }

    private void Update()
    {
        bb.isInRange = bb.distanceToTarget <= cfg.preferredMax && bb.distanceToTarget >= cfg.preferredMin;
    }

    public void Listen(Vector3 pos)
    {
        Debug.Log("Listened");
        if (!bb.hasLOS)
        {
            bb.lastKnownTargetPos = pos;
            bb.InvestigateSound = true;
        }
    }

}
