using UnityEngine;
using System;

[Serializable]
public class B_Action
{
    [HideInInspector] public EnemyBrain brain;

    public b_Action B_action;
    public bool Override;
    public float Weight;

    public enum b_Action
    {
        DoThing , 
        ChangeToPatrol, 
        ChangeToIdle, 
        MoveToPlayer, 
        MoveToPatrol,
        MoveToLSP,
        SearchNearby,
        AttackSet, 
        Attack,
        DashToPlayer
    }

    
    public Action Func()
    {
        switch (B_action)
        {
            case b_Action.DoThing:
                return brain.actionHandler.DoLilHops;
            case b_Action.MoveToPlayer:
                return brain.actionHandler.MoveToPlayer;
            case b_Action.MoveToPatrol:
                return brain.actionHandler.DoPatrol;
            case b_Action.ChangeToPatrol:
                return brain.actionHandler.ChangeToPatrolState;
            case b_Action.ChangeToIdle:
                return brain.actionHandler.ChangeToIdleState;
            case b_Action.MoveToLSP:
                return brain.actionHandler.MoveToLSP;
            case b_Action.SearchNearby:
                return brain.actionHandler.SearchNearby;
            case b_Action.DashToPlayer:
                return brain.actionHandler.DashToPlayer;
            default:
                return brain.actionHandler.DoError;
        }
    }


    
}
