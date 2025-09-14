using UnityEngine;

public class EnemyAttackState : BaseState
{
    bool _locked;
    public EnemyAttackState(EnemyContext ctx) : base(ctx)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        _locked = true;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void Tick()
    {
        base.Tick();
    }


    public override bool CanBeInterruptedBy(IState next)
    {
        return !_locked;
    }

}
