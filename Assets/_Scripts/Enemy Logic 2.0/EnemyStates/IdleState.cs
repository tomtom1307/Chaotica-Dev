using UnityEngine;

public class IdleState : BaseState
{
    

    public IdleState(EnemyContext c) : base(c) { }

    public override IState.StateAggroType stateAggroType => IState.StateAggroType.NonAggro;

    public override void OnEnter() { 
        base.OnEnter();
        c.bb.isAggro = false; }
    public override void Tick()
    {
        base.Tick();
        c.motor.SetVelocity(Vector3.zero);
        c.motor.SetAltitude(c.cfg.altitudeOffset, null);
        if (c.bb.DetectionMeter == 1) c.aimer.AimAt(c.bb.target.position);
    }

}
