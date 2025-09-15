using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;


//Holds all references
[RequireComponent(typeof(Aimer))]
[RequireComponent(typeof(EAI_Perception))]
[RequireComponent (typeof(DamagableEnemy))]
[RequireComponent (typeof(EAI_AnimatorController))]
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
    [HideInInspector] public Animator animator;
    [HideInInspector] public AnimatorController anim;

    void Awake()
    {
        

        bb = gameObject.AddComponent<EAI_Blackboard>();
        stateMachine = gameObject.AddComponent<EAI_StateMachine>();
        abilities = gameObject.AddComponent<EnemyAbilityRunner>();

        motor = GetComponent<IGoalMotor>();
        if (motor == null) Debug.LogWarning("Enemy Does not have a motor attached, please attach one");
        aimer = GetComponent<Aimer>();
        sense = GetComponent<EAI_Perception>();
        anim = GetComponent<AnimatorController>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        bb.isInRange = bb.distanceToTarget <= cfg.preferredMax && bb.distanceToTarget >= cfg.preferredMin;

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
