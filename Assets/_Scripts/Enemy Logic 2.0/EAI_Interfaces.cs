using System;
using UnityEngine;

public interface IMotor
{
    float MaxSpeed { get; set; }
    bool Enabled { get; set; }   // brain can disable during stun/KB
    void SetVelocity(Vector3 worldVel);               // XZ intent
    void SetAltitude(float height, Transform rel = null);   // ignore on ground

    void EnablePhysics(bool x);
}

public interface IGoalMotor : IMotor
{

    // Goal-based control (works for both ground + hover).
    bool ReachedGoal { get; }
    Vector3 Destination { get; }

    Vector3 Velocity { get;}
    void MoveTo(Vector3 dest, float stopDistance = 0.25f, float? altitude = null);
    void Stop();

    // Path test: true for hover (always can), or NavMesh.CalculatePath for ground.
    bool CanPathTo(Vector3 dest);


    void SetPosition(Vector3 dest);
}

public interface IAbilityRunner
{
    bool CanUse(AbilityEntry ability);
    void Use(AbilityEntry ability, in AbilityContext ctx);
}

public interface IPerception
{
    Transform PrimaryTarget { get; }
    bool HasLOS(Vector3 from, Vector3 to, float radius);

    float DistanceToTarget { get; }
}

public interface IKnockbackable
{
    void GetKnockedBack(Vector3 force, ForceMode forceMode = ForceMode.Force);
    void GetKnockedBack(Vector3 force, Vector3 point);
}

