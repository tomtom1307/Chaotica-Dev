using UnityEngine;

public interface IMotor
{
    float MaxSpeed { get; set; }
    bool Enabled { get; set; }   // brain can disable during stun/KB
    void SetVelocity(Vector3 worldVel);               // XZ intent
    void SetAltitude(float? height, Transform rel);   // ignore on ground
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

