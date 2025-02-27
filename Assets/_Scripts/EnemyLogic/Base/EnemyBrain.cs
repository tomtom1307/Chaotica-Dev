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

    public Dictionary<CheckBool.Bool, bool> conditionBools;
    public Dictionary<CheckFloat.Float, float> conditionFloats;
    public Dictionary<CheckInt.Int, int> conditionInts;
    private void Start()
    {
        damagableEnemy = GetComponent<DamagableEnemy>();
        stateMachine = new EnemyStateMachine();
        idleState = new IdleState(damagableEnemy, stateMachine, behaviours);
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }

    public void StartRepeatingChecks(List<string> checks)
    {
        foreach (string checkmethod in checks)
        {
            //InvokeRepeating(checkmethod, );
        }
    }
    public void StopRepeatingChecks(List<string> checks)
    {
        foreach (string checkmethod in checks)
        {
            //CancelInvoke(checkmethod, );
        }
    }

    private bool checkLOS() { return false; }
    private float checkDistance() { return 0f; }

}
