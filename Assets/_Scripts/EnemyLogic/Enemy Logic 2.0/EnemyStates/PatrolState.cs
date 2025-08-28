using UnityEngine;

public class PatrolState : BaseState
{
    public new string Name => "Patrol";
    EnemyContext c;

    public PatrolState(EnemyContext ctx) : base(ctx)
    {
    }

    public override void OnEnter()
    {
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
