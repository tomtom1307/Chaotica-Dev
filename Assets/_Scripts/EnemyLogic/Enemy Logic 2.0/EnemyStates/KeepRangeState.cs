using UnityEngine;

public class KeepRangeState : BaseState
{

    public new string Name => "KeepRange";

    readonly EnemyContext c;
    public KeepRangeState(EnemyContext c) { this.c = c; }

    public override void OnEnter() {}
    public override void Tick()
    {
        var vel = SteeringHelpers.KeepRange(c.transform, c.bb.target, c.cfg.approachSpeed, c.cfg.preferredMin, c.cfg.preferredMax);
        c.motor.SetVelocity(vel);
        c.motor.SetAltitude(c.cfg.altitudeOffset, c.bb.target);
        if (c.bb.target) c.aimer.AimAt(c.bb.target.position + Vector3.up * 0.5f);
    }
    public override void OnExit() { }
    public override IState Next()
    {
        bool inBand = c.bb.distanceToTarget >= c.cfg.preferredMin && c.bb.distanceToTarget <= c.cfg.preferredMax;
        return inBand ? new OrbitAttackState(c) : null;
    }
}
