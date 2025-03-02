using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(DamagableEnemy))]
public class EnemyBrain : MonoBehaviour
{

    [Header("Movement")]
    public float MoveSpeed;
    public float JumpPower;

    [Header("Detection")]
    public float DetectionRange;
    public float PerceptionStat;
    public float DetectionMeterDecay;
    public float ViewAngle;
    public LayerMask layerMask;





    //Navmesh
    [HideInInspector] public NavMeshAgent navMesh;

    //StateMachine
    EnemyStateMachine stateMachine;
    IdleState idleState;
    //add more concrete state classes here


    //Behaviours
    [SerializeField] List<EnemyBehaviour> behaviours;
    [HideInInspector] public EnemyPerception perception;
    [HideInInspector] public EnemyActionHandler actionHandler;

    //Health
    private DamagableEnemy damagableEnemy;







    //Stores the CheckBool value
    public Dictionary<CheckBool.Bool, Func<bool>> conditionBools = new Dictionary<CheckBool.Bool, Func<bool>>() { };

    public void MapBools()
    {
        conditionBools[CheckBool.Bool.LOS] = () => perception.LOS;
    }


    //Stores the CheckFloat value
    public Dictionary<CheckFloat.Float, Func<float>> conditionFloats = new Dictionary<CheckFloat.Float, Func<float>>() { };

    public void MapFloats()
    {
        conditionFloats[CheckFloat.Float.LSP_time] = () => perception.LSP_time;
        conditionFloats[CheckFloat.Float.playerDistance] = () => perception.Distance;
        conditionFloats[CheckFloat.Float.DetectionMeter] = () => perception.DetectionMeter;
    }

    //Stores the CheckInt value
    public Dictionary<CheckInt.Int, Func<int>> conditionInts = new Dictionary<CheckInt.Int, Func<int>>() { };

    public void MapInts()
    {

    }

    private void Start()
    {
        //Get references 
        actionHandler = gameObject.AddComponent<EnemyActionHandler>();
        actionHandler.brain = this;
        perception = gameObject.AddComponent<EnemyPerception>();
        perception.brain = this;
        damagableEnemy = GetComponent<DamagableEnemy>();
        

        //Initialize State machine + states
        stateMachine = new EnemyStateMachine();
        idleState = new IdleState(damagableEnemy, stateMachine, behaviours, this);
        stateMachine.Initialize(idleState);

        //Do Dict mappings
        MapBools();
        MapFloats();
        MapInts();

    }

    private void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }


}
