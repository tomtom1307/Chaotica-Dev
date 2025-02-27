using UnityEngine;

public class EnemyState     
{
    protected Damagable damagable;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(Damagable damagable, EnemyStateMachine enemyStateMachine)
    {
        this.damagable = damagable;
        this.enemyStateMachine = enemyStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent() { } 
}
