using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    EnemyStateMachine stateMachine;
    EnemyState idleState;
    private void Start()
    {
        stateMachine = new EnemyStateMachine();
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        
    }
}
