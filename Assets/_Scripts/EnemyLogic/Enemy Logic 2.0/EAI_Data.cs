using UnityEngine;

// File: AI/Data/EnemyTuningSO.cs
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Enemy Tuning")]
public class EnemyTuningSO : ScriptableObject
{
    [Header("Range band")]
    public float preferredMin = 10f;
    public float preferredMax = 16f;

    [Header("Motion")]
    public float approachSpeed = 8f;
    public float orbitSpeedRad = 1.3f;
    public float strafeWeight = 1.0f;
    public float altitudeOffset = 3.5f; // flyers

    [Header("Combat")]
    public AbilitySO primaryAbility;
    public int burstCount = 3;
    public float timeBetweenShots = 0.12f;
    public float burstInterval = 2.2f;
    public float burstJitter = 0.4f;
    public float losRadius = 0.25f;
}

public abstract class AbilitySO : ScriptableObject
{
    public string abilityId = "AbilityID";
    public float cooldown = 1.0f;
    public abstract void Execute(GameObject owner, in AbilityContext ctx);
}

public struct AbilityContext
{
    public Vector3 origin;
    public Vector3 direction;
    public Transform target;
}
