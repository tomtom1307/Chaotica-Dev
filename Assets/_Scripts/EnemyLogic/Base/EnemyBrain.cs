using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
/// <summary>
/// The <c>EnemyBrain</c> class is a component attached to every in-game enemy. It handles all of the enemy's behaviour.
/// <para>
/// It holds the appropriate instances of the <c>EnemyStateMachine</c>, <c>EnemyState</c> and <c>EnemyPerception</c>.
/// </para>
/// </summary>
[RequireComponent(typeof(DamagableEnemy))]
[RequireComponent(typeof(EnemyAnimationEventHandler))]
public class EnemyBrain : MonoBehaviour
{

    [Header("Movement")]
    ///<value> Property <c>MoveSpeed</c> holds the enemy's navmesh agent speed.</value>
    public float MoveSpeed;
    public float JumpPower;

    [Header("Detection")]
    public float DetectionRange;
    public float PerceptionStat;
    public float DetectionMeterDecay;
    public float ViewAngle;
    public LayerMask layerMask;
    public Transform LookDirectionTransform;
    EnemyAnimationEventHandler enemyAnimationevent;
    //Navmesh
    [HideInInspector] public NavMeshAgent navMesh;

    [Header("State Machine")]
    public EnemyStateMachine stateMachine;
    public string currentStateName;
    [SerializeField] public List<EnemyState> stateList; // Appears in inspector and this is where behaviours are implemented
    [HideInInspector] public EnemyState idleState; //individual class for each state seemingly uneccesary as we can handle any specific EnterState()/ExitState() things as actions
    [HideInInspector] public EnemyState patrolState;
    //add more concrete state classes here
    [HideInInspector] public EnemyPerception perception;
    [HideInInspector] public EnemyActionHandler actionHandler;
    [HideInInspector] public EnemyAttackHandler attackHandler;


    [HideInInspector] public UnityEvent OnPlayerSpotted;
    [HideInInspector] public UnityEvent OnPlayerLost;
    //Health
    private DamagableEnemy damagableEnemy;

    [Header("Debug")]
    public bool stateChangeDebug;

    //Animator
    [HideInInspector] public Animator animator;



    //Stores the CheckBool value
    public Dictionary<CheckBool.Bool, Func<bool>> conditionBools = new Dictionary<CheckBool.Bool, Func<bool>>() { };

    public virtual void MapBools()
    {
        conditionBools[CheckBool.Bool.LOS] = () => perception.LOS;
    }


    //Stores the CheckFloat value
    public Dictionary<CheckFloat.Float, Func<float>> conditionFloats = new Dictionary<CheckFloat.Float, Func<float>>() { };

    public virtual void MapFloats()
    {
        conditionFloats[CheckFloat.Float.LSP_time] = () => perception.LSP_time;
        conditionFloats[CheckFloat.Float.playerDistance] = () => perception.Distance;
        conditionFloats[CheckFloat.Float.DetectionMeter] = () => perception.DetectionMeter;
    }

    //Stores the CheckInt value
    public Dictionary<CheckInt.Int, Func<int>> conditionInts = new Dictionary<CheckInt.Int, Func<int>>() { };

    public virtual void MapInts()
    {

    }



    protected virtual void Start()
    {


        if (LookDirectionTransform == null) LookDirectionTransform = this.transform;

        //Get references 
        

        actionHandler = gameObject.AddComponent<EnemyActionHandler>();
        actionHandler.brain = this;
        perception = gameObject.AddComponent<EnemyPerception>();
        perception.brain = this;

        attackHandler = gameObject.AddComponent<EnemyAttackHandler>();
        attackHandler.Init(this);

        enemyAnimationevent = GetComponent<EnemyAnimationEventHandler>();
        enemyAnimationevent.attackHandler = attackHandler;

        damagableEnemy = GetComponent<DamagableEnemy>();
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = MoveSpeed;
        animator = GetComponent<Animator>();





        //Initialize State machine + states
        stateMachine = new EnemyStateMachine(this);
        idleState = new EnemyState(damagableEnemy, stateMachine, stateList[0].enemyBehaviours, stateList[0].Name, this);
        patrolState = new EnemyState(damagableEnemy, stateMachine, stateList[1].enemyBehaviours, stateList[1].Name, this);
        stateMachine.Initialize(idleState);

        //Do Dict mappings
        MapBools();
        MapFloats();
        MapInts();

    }

    protected virtual void Update()
    {
        navMesh.speed = MoveSpeed;
        stateMachine.CurrentEnemyState.FrameUpdate();

    }


}
