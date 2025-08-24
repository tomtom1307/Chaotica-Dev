using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OrbitAttackState : BaseState
{
    public new string Name => "AttackOrbit";

    readonly EnemyContext c;
    float angle;          // state-owned orbit phase
    int shotsLeft;
    float nextShotAt;

    public OrbitAttackState(EnemyContext c) { this.c = c; angle = Random.Range(0f, Mathf.PI * 2f); }

    public override void OnEnter()
    {
        angle = Random.Range(0f, Mathf.PI * 2f);
        shotsLeft = 0; nextShotAt = 0f; 
        c.nextBurstAt = Time.time + c.cfg.burstInterval + Random.Range(0f, c.cfg.burstJitter);
    }

    public override void Tick()
    {
        // Orbit velocity (seek ring + strafe)
        var self = c.transform;
        var target = c.bb.target;
        Vector3 vel = Vector3.zero;
        OrbitLogic(target, self, vel);
        

        c.motor.SetVelocity(vel);
        c.motor.SetAltitude(c.cfg.altitudeOffset, c.bb.target);

        
    }

    public override void OnExit() { }

    public override IState Next()
    {
        bool outOfBand = c.bb.distanceToTarget < c.cfg.preferredMin || c.bb.distanceToTarget > c.cfg.preferredMax;
        return outOfBand ? new KeepRangeState(c) : null;
    }


    public void OrbitLogic(Transform target, Transform self, Vector3 vel)
    {
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
        }
    }
}

