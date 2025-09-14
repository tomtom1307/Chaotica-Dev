using UnityEngine;

public class PatrolState : BaseState
{
    public new string Name => "Patrol";
    EnemyContext c;

    public override IState.StateAggroType stateAggroType => IState.StateAggroType.NonAggro;

    public PatrolState(EnemyContext ctx) : base(ctx)
    {
    }

    public override void OnEnter()
    {
        c.bb.isAggro = false;
        base.OnEnter();
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}
