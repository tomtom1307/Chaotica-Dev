using DG.Tweening;
using UnityEngine;

public class EnemyActionHandler : MonoBehaviour
{
    [HideInInspector] public EnemyBrain brain;
    public bool DoingAction;

    public void StartAction(System.Action someFunction)
    {
        if (!DoingAction)
        {
            DoingAction = true; // Set the action flag to true, so no other actions can be started

            someFunction.Invoke();
        }
    }

    public void EndAction()
    {
        DoingAction = false;
    }

    public void DoLilHops()
    {
        transform.DOJump(transform.position, brain.JumpPower, 7, 1).OnComplete(EndAction);
    }
    public void ChangeToIdleState()
    {
        brain.stateMachine.ChangeState(brain.stateList[0]); Debug.Log("Changed to Idle State");
        EndAction();
    }
    public void ChangeToPatrolState()
    {
        brain.stateMachine.ChangeState(brain.stateList[1]); Debug.Log("Changed to Patrol State");
        EndAction();
    }

    public void DoError()
    {
        Debug.LogError("This action does not exist. Look at EnemyActionHandler.cs");
    }


    

}
