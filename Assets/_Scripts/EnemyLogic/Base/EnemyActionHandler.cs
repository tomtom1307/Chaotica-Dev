using DG.Tweening;
using System;
using UnityEngine;

public class EnemyActionHandler : MonoBehaviour
{
    [HideInInspector] public EnemyBrain brain;
    public bool DoingAction;


    private void Start()
    {
        DefaultStoppingDistance = brain.navMesh.stoppingDistance;
    }

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
    public void MoveToPlayer()
    {
        brain.navMesh.stoppingDistance = DefaultStoppingDistance;
        brain.navMesh.destination = brain.perception.player.position;
        brain.animator.SetBool("Walking", true);
        EndAction();
    }
    public void ChangeToIdleState()
    {
        brain.stateMachine.ChangeState(brain.idleState); Debug.Log("Changed to Idle State");
        EndAction();
    }
    public void ChangeToPatrolState()
    {
        brain.stateMachine.ChangeState(brain.patrolState); Debug.Log("Changed to Patrol State");

        brain.OnPlayerSpotted.Invoke(); //TODO Handle in seperate action or include in change to attack state logic


        EndAction();
    }


    public EnemyPatrolPoint TargetpatrolPoint;
    public float DefaultStoppingDistance;

    public void DoPatrol()
    {
        brain.navMesh.stoppingDistance = 0;
        if(TargetpatrolPoint == null)
        {
            TargetpatrolPoint = GameManager.instance.FindClosestPatrol(transform.position);
            if (TargetpatrolPoint == null) { return; }
        }
        TargetpatrolPoint.Claim();
        brain.navMesh.SetDestination(TargetpatrolPoint.transform.position);
        if(brain.navMesh.remainingDistance < 0.2f)
        {
            //TODO: Implement WaitTimeLogic and get next Available patrol point
            TargetpatrolPoint.MoveOn();
            TargetpatrolPoint = TargetpatrolPoint.nextInPatrol;

        }

        EndAction();
    }


    public void DoError()
    {
        Debug.LogError("This action does not exist. Look at EnemyActionHandler.cs");
    }

    
}
