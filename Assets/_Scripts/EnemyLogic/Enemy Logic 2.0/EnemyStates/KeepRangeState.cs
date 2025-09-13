using UnityEngine;

public class KeepRangeState : BaseState
{
    public KeepRangeState(EnemyContext c) : base(c) { }

    public override void OnEnter() {}
    public override void Tick()
    {
        
        var pos = SteeringHelpers.KeepRange(c.transform, c.bb.target, c.cfg.preferredMin, c.cfg.preferredMax);
        c.motor.MoveTo(pos);
        c.motor.SetAltitude(c.cfg.AltitudeFromPlayer, c.bb.target);
        if (c.bb.hasLOS) c.aimer.AimAt(c.bb.target.position + Vector3.up * 0.5f);
        else c.aimer.AimAt(c.transform.position + c.motor.Velocity);
    }
    public override void OnExit() { }
}
