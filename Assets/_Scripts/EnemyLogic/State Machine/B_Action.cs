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
        DoThing
    }

    
    public Action Func()
    {
        switch (B_action)
        {
            case b_Action.DoThing:
                return brain.actionHandler.DoAFlip;
            default:
                return brain.actionHandler.DoError;
        }
    }


    
}
