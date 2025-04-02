using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEditor.Handles;

public class EnemyActionHandler : MonoBehaviour
{
    [HideInInspector] public EnemyBrain brain;
    public bool DoingAction;
    public string CurrentAction;

    private void Start()
    {
        DefaultStoppingDistance = brain.navMesh.stoppingDistance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeToIdleState();
        }
        if (Searching)
        {
            if(brain.navMesh.remainingDistance <1) { Invoke(nameof(EndAction), 1); }
        }
    }

    public void StartAction(System.Action someFunction)
    {
        if (!DoingAction)
        {
            DoingAction = true; // Set the action flag to true, so no other actions can be started
            CurrentAction = someFunction.Method.Name;
            someFunction.Invoke();
        }
    }


    public void StartActionOverride(Action someFunction)
    {
        DoingAction = true;
        someFunction.Invoke();
    }

    public void EndAction()
    {
        CurrentAction = "";
        DoingAction = false;
    }

    public void DoLilHops()
    {
        transform.DOJump(transform.position, brain.JumpPower, 7, 1).OnComplete(EndAction);
    }
    public void MoveToPlayer()
    {
        brain.navMesh.stoppingDistance = DefaultStoppingDistance;
        brain.navMesh.SetDestination(brain.perception.player.position);
        brain.animator.SetBool("Walking", true);
        EndAction();
    }
    public void ChangeToIdleState()
    {
        if(brain.stateChangeDebug) Debug.Log("Changed to Idle State");
        brain.stateMachine.ChangeState(brain.idleState); 
        EndAction();
    }
    public void ChangeToPatrolState()
    {
        if(brain.stateChangeDebug) Debug.Log("Changed to Patrol State");
        brain.stateMachine.ChangeState(brain.patrolState); 

        brain.OnPlayerSpotted.Invoke(); //TODO Handle in seperate action or include in change to attack state logic


        EndAction();
    }


    public EnemyPatrolPoint TargetpatrolPoint;
    public float DefaultStoppingDistance;

    public void MoveToLSP()
    {
        brain.navMesh.stoppingDistance = 0;
        brain.animator.SetBool("Walking", true);
        brain.navMesh.SetDestination(brain.perception.PlayerLastSeenPosition);
        SearchNearby();
        Searching = true;
    }

    public void DoPatrol()
    {
        brain.navMesh.stoppingDistance = 0;
        if(TargetpatrolPoint == null)
        {
            TargetpatrolPoint = GameManager.instance.FindClosestPatrol(transform.position);
            if (TargetpatrolPoint == null) { return; }
        }
        TargetpatrolPoint.Claim();
        brain.animator.SetBool("Walking", true);
        brain.navMesh.SetDestination(TargetpatrolPoint.transform.position);
        if(brain.navMesh.remainingDistance < 0.2f)
        {
            //TODO: Implement WaitTimeLogic and get next Available patrol point
            TargetpatrolPoint.MoveOn();
            TargetpatrolPoint = TargetpatrolPoint.nextInPatrol;

        }

        EndAction();
    }

    bool Searching;
    public void SearchNearby()
    {
        brain.navMesh.stoppingDistance = 0;
        float Range = Vector3.Distance(brain.perception.player.position, brain.perception.PlayerLastSeenPosition) * brain.SearchRange ;

        float Random1 = UnityEngine.Random.Range(-Range, Range);
        float Random2 = UnityEngine.Random.Range(-Range, Range);
        Vector3 SearchPos = brain.perception.player.position + Random1*Vector3.forward + Random2*Vector3.right;

        brain.navMesh.SetDestination(SearchPos);
        brain.animator.SetBool("Walking", true);
        Searching = true;
    }


    public void DoError()
    {
        Debug.LogError("This action does not exist. Look at EnemyActionHandler.cs");
    }

    
}
