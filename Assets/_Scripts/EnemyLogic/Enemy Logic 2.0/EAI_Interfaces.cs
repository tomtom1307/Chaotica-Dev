using System;
using UnityEngine;

public interface IMotor
{
    float MaxSpeed { get; set; }
    bool Enabled { get; set; }   // brain can disable during stun/KB
    void SetVelocity(Vector3 worldVel);               // XZ intent
    void SetAltitude(float? height, Transform rel);   // ignore on ground
}

[Flags]
public enum MotorCaps { None = 0, Destination = 1, Pathfinding = 2, Hover3D = 4 }
public interface IGoalMotor : IMotor
{
    MotorCaps Caps { get; }

    // Goal-based control (works for both ground + hover).
    bool ReachedGoal { get; }
    Vector3 Destination { get; }

    
    void MoveTo(Vector3 dest, float stopDistance = 0.25f, float? altitude = null);
    void Follow(Transform target, float updateDist = 0.75f, float updateSeconds = 0.2f, float? altitudeOffset = null);
    void Stop();

    // Path test: true for hover (always can), or NavMesh.CalculatePath for ground.
    bool CanPathTo(Vector3 dest);
}

public interface IAbilityRunner
{
    bool CanUse(AbilitySO ability);
    void Use(AbilitySO ability, in AbilityContext ctx);
}

public interface IPerception
{
    Transform PrimaryTarget { get; }
    bool HasLOS(Vector3 from, Vector3 to, float radius);
    float DistanceToTarget { get; }
}

