using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(Damagable damagable, EnemyStateMachine enemyStateMachine) : base(damagable, enemyStateMachine) { }



    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }
}
