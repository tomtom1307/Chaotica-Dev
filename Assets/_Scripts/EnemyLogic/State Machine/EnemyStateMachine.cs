using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentEnemyState { get; set; }
    private EnemyBrain brain;
    public EnemyStateMachine(EnemyBrain brain)
    {
        this.brain = brain;
    }

    public void Initialize(EnemyState startingState)
    {
        CurrentEnemyState = startingState;
        CurrentEnemyState.EnterState();
        brain.currentStateName = CurrentEnemyState.Name;
        Debug.Log("set currentStateName to " + CurrentEnemyState.Name);
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentEnemyState.ExitState();
        CurrentEnemyState = newState;
        CurrentEnemyState.EnterState();
        brain.currentStateName = CurrentEnemyState.Name;
    }
}
