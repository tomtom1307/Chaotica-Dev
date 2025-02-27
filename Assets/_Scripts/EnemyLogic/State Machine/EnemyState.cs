using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState     
{
    protected Damagable damagable;
    protected EnemyStateMachine enemyStateMachine;
    protected List<EnemyBehaviour> enemyBehaviours;

    public EnemyState(Damagable damagable, EnemyStateMachine enemyStateMachine, List<EnemyBehaviour> enemyBehaviours)
    {
        this.damagable = damagable;
        this.enemyStateMachine = enemyStateMachine;
        this.enemyBehaviours = enemyBehaviours;
    }

    public virtual void EnterState() 
    { 
        
    }
    public virtual void ExitState() 
    {
        
    }
    public virtual void FrameUpdate() 
    {
        foreach (EnemyBehaviour behaviour in enemyBehaviours)
        {
            behaviour.conditions.CheckConditions();
        }

    }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent() { } 
}
