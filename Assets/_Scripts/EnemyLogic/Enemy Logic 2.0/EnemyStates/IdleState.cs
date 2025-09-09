using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(EnemyContext c) : base(c) { }
    public override void OnEnter() {}
    public override void Tick()
    {
        c.motor.SetVelocity(Vector3.zero);
        
        if (c.bb.DetectionMeter == 1) c.aimer.AimAt(c.bb.target.position);

    }
}
