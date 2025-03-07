using UnityEngine;
using System;

[Serializable]
public class B_Action
{
    [HideInInspector] public EnemyBrain brain;

    public b_Action B_action;
    public float Weight;

    public enum b_Action
    {
        DoThing , ChangeToPatrol, ChangeToIdle, MoveToPlayer, MoveToPatrol, AttackSet, Attack
    }

    
    public Action Func()
    {
        switch (B_action)
        {
            case b_Action.DoThing:
                return brain.actionHandler.DoLilHops;
            case b_Action.MoveToPlayer:
                return brain.actionHandler.MoveToPlayer;
            case b_Action.ChangeToPatrol:
                return brain.actionHandler.ChangeToPatrolState;
            case b_Action.ChangeToIdle:
                return brain.actionHandler.ChangeToIdleState;
            default:
                return brain.actionHandler.DoError;
        }
    }


    
}
