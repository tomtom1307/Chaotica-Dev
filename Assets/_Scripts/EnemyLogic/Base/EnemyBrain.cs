using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DamagableEnemy))]
public class EnemyBrain : MonoBehaviour
{
    EnemyStateMachine stateMachine;
    IdleState idleState;
    //add more concrete state classes here
    [SerializeField] List<EnemyBehaviour> behaviours;
    private DamagableEnemy damagableEnemy;
    public EnemyPerception perception;

    public Dictionary<CheckBool.Bool, bool> conditionBools = new Dictionary<CheckBool.Bool, bool>()
    {
        // Need to find a way for the dictionary to provide a reference to the variable rather than the value of the variable at the time of this definition
    };
    public Dictionary<CheckFloat.Float, float> conditionFloats;
    public Dictionary<CheckInt.Int, int> conditionInts;
    private void Start()
    {
        perception = gameObject.AddComponent<EnemyPerception>();
        damagableEnemy = GetComponent<DamagableEnemy>();
        stateMachine = new EnemyStateMachine();
        idleState = new IdleState(damagableEnemy, stateMachine, behaviours, this);
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }


}
