using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;


//Holds all references
[RequireComponent(typeof(Aimer))]
[RequireComponent(typeof(EAI_Perception))]
[RequireComponent (typeof(DamagableEnemy))]
[RequireComponent (typeof(EAI_AttackHandler))]
[RequireComponent (typeof(EAI_AnimatorController))]
public class EnemyContext : MonoBehaviour
{
    public EnemyTuningSO cfg;
    public Transform muzzle;
    public Transform LookDirection;
    public bool DebugStates;
    [HideInInspector] public EAI_Blackboard bb;
    [HideInInspector] public IGoalMotor motor;
    [HideInInspector] public Aimer aimer;
    [HideInInspector] public EnemyAbilityRunner abilityRunner;
    [HideInInspector] public EAI_Perception sense;
    [HideInInspector] public EAI_StateMachine stateMachine;
    [HideInInspector] public EAI_AnimatorController anim;
    [HideInInspector] public DamagableEnemy Health;
    [HideInInspector] public EAI_AttackHandler attackHandler;
    void Awake()
    {

        if (LookDirection == null) LookDirection = transform;
        bb = gameObject.AddComponent<EAI_Blackboard>();

        abilityRunner = gameObject.AddComponent<EnemyAbilityRunner>();

        motor = GetComponent<IGoalMotor>();
        if (motor == null) Debug.LogWarning("Enemy Does not have a motor attached, please attach one");
        aimer = GetComponent<Aimer>();
        sense = GetComponent<EAI_Perception>();
        sense.Init();
        anim = GetComponent<EAI_AnimatorController>();
        Health = GetComponent<DamagableEnemy>();
        attackHandler = GetComponent<EAI_AttackHandler>();
        stateMachine = gameObject.AddComponent<EAI_StateMachine>();
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
