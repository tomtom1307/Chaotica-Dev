using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OrbitAttackState : BaseState
{
    float angle;          // state-owned orbit phase
    

    public OrbitAttackState(EnemyContext c) : base(c) { angle = Random.Range(0f, Mathf.PI * 2f); }

    public override void OnEnter()
    {
        angle = Random.Range(0f, Mathf.PI * 2f);
    }

    public override void Tick()
    {
        // Orbit velocity (seek ring + strafe)
        var self = c.transform;
        var target = c.bb.target;
        var vel = OrbitLogic(target, self);
        c.motor.SetVelocity(vel);
        c.motor.SetAltitude(c.cfg.altitudeOffset, c.bb.target);
    }

    public override void OnExit() { }



    public Vector3 OrbitLogic(Transform target, Transform self)
    {
        Vector3 vel = Vector3.zero;
        if (target)
        {
            
            angle += c.cfg.orbitSpeedRad * Time.deltaTime;

            Vector3 toT = target.position - self.position;
            float d = Mathf.Max(c.bb.distanceToTarget, 0.001f);
            float clampR = Mathf.Clamp(d, c.cfg.preferredMin, c.cfg.preferredMax);

            Vector3 ring = target.position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * clampR;
            Vector3 seek = (ring - self.position).normalized * c.cfg.approachSpeed;
            Vector3 strafe = Vector3.Cross(Vector3.up, toT.normalized) * c.cfg.approachSpeed * c.cfg.strafeWeight;

            // band correction
            if (d < c.cfg.preferredMin) seek += (-toT.normalized) * c.cfg.approachSpeed;
            else if (d > c.cfg.preferredMax) seek += (toT.normalized) * c.cfg.approachSpeed;

            var v = seek + strafe;
            vel = new Vector3(v.x, 0f, v.z);
            
            c.aimer.AimAt(target.position + Vector3.up * 0.5f);
            return vel;
        }
        return Vector3.zero;
    }
}

