using UnityEngine;

public class IdleState : BaseState
{
    public new string Name => "Idle";


    readonly EnemyContext c;
    public IdleState(EnemyContext c) { this.c = c; }
    public override void OnEnter() {}
    public override void Tick()
    {
        c.motor.SetVelocity(Vector3.zero);
        
        if (c.bb.DetectionMeter == 1) c.aimer.AimAt(c.bb.target.position);

    }
    public override void OnExit() { }
    public override IState Next() => c.bb.DetectionMeter == 1 ? new KeepRangeState(c) : null;
}
