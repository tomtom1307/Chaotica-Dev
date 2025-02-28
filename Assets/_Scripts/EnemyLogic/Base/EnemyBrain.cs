using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(DamagableEnemy))]
public class EnemyBrain : MonoBehaviour
{

    [Header("Movement")]
    public float MoveSpeed;

    [Header("Detection")]
    public float DetectionRange;
    public LayerMask layerMask;








    //StateMachine
    EnemyStateMachine stateMachine;
    IdleState idleState;
    //add more concrete state classes here


    //Behaviours
    [SerializeField] List<EnemyBehaviour> behaviours;
    [HideInInspector] public EnemyPerception perception;

    //Health
    private DamagableEnemy damagableEnemy;







    //Stores the CheckBool value
    public Dictionary<CheckBool.Bool, Func<bool>> conditionBools = new Dictionary<CheckBool.Bool, Func<bool>>() { };

    public void MapBools()
    {
        conditionBools[CheckBool.Bool.LOS] = () => perception.LOS;
    }


    //Stores the CheckFloat value
    public Dictionary<CheckFloat.Float, float> conditionFloats;

    public void MapFloats()
    {

    }

    //Stores the CheckInt value
    public Dictionary<CheckInt.Int, int> conditionInts;

    public void MapInts()
    {

    }

    private void Start()
    {
        //Get references 
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
