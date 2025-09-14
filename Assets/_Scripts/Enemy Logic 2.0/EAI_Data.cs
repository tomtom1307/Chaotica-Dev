using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


[CreateAssetMenu(menuName = "AI/Enemy Tuning")]
public class EnemyTuningSO : ScriptableObject
{
    [Header("Range band")]
    public float preferredMin = 10f;
    public float preferredMax = 16f;
    public float DefaultGroundAltitude = 4;
    public float AltitudeFromPlayer = 2;

    [Header("Motion")]
    public float approachSpeed = 8f;
    public float orbitSpeedRad = 1.3f;
    public float strafeWeight = 1.0f;
    public float altitudeOffset = 3.5f; // flyers

    [Header("Combat")]
    public float BaseDamage;
    public List<EAI_AttackSO> Attacks;
    public List<EAI_AbilitySO> Abilities;

    [Header("Detection")]
    public float SearchDuration = 6;
    public float SearchRingRadius = 3;
    public int searchLookSegments = 6;
    public float searchDwell = 0.5f;
    public int searchRingSteps = 3;
    public float searchSpeedMult = 0.9f;
    public float searchArrivalDistance = 1;
    public float GoBackToSearchTime;



}

public abstract class EAI_AbilitySO : ScriptableObject
{
    public string abilityId = "AbilityID";
    public float cooldown = 1.0f;
    public abstract void Enter(GameObject owner, in AbilityContext ctx);
    public abstract void Execute(GameObject owner, in AbilityContext ctx);
    public abstract void Exit(GameObject owner, in AbilityContext ctx);
}

public class EAI_AttackSO : EAI_AbilitySO
{
    public float Damage;

    public override void Enter(GameObject owner, in AbilityContext ctx)
    {
        
    }

    public override void Execute(GameObject owner, in AbilityContext ctx)
    {
        
    }

    public override void Exit(GameObject owner, in AbilityContext ctx)
    {
        
    }
}


public struct AbilityContext
{
    public Vector3 origin;
    public Vector3 direction;
    public Transform target;
}
