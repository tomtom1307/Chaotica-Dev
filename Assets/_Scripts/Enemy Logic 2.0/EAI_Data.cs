using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


[CreateAssetMenu(menuName = "AI/Enemy Tuning")]
public class EnemyTuningSO : ScriptableObject
{
    


    [Header("Range band")]
    public float preferredMin = 10f;
    public float preferredMax = 16f;
    public float AltitudeFromPlayer = 2;

    [Header("Motion")]
    public float approachSpeed = 8f;
    public float orbitSpeedRad = 1.3f;
    public float strafeWeight = 1.0f;
    public float DefaultAltitude = 3.5f; // flyers

    [Header("Combat")]
    public float MaxHealth = 20;
    public float BaseDamage = 5;
    public List<AbilityEntry> Attacks;
    public List<AbilityEntry> Abilities;

    [Header("Detection")]
    public float SearchDuration = 6;
    public float SearchRingRadius = 3;
    public int searchLookSegments = 6;
    public float searchDwell = 0.5f;
    public int searchRingSteps = 3;
    public float searchSpeedMult = 0.9f;
    public float searchArrivalDistance = 1;
    public float GoBackToSearchTime = 10;



}

public abstract class EAI_AbilitySO : ScriptableObject
{
    public string abilityId = "AbilityID";
    public float active_duration = 1.0f;
    public float cooldown = 1.0f;
    public abstract void Enter(EnemyContext EC);
    public abstract void Execute(EnemyContext EC);
    public abstract void Exit(EnemyContext EC);
    public abstract void Tick(EnemyContext EC);

    public abstract void LateTick(EnemyContext EC);
}




public struct AbilityContext
{
    public Transform target;
    public Vector3 targetPos;

    public AbilityContext(Transform target)
    {
        this.target = target;
        targetPos = Vector3.zero;
    }

    public AbilityContext(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        this.target = null;
    }
}


[System.Serializable]
public struct AbilityEntry
{
    public string Name;
    public EAI_AbilitySO Ability;      // the definition
    public bool Enabled;               // designer toggle
    public float Weight;               // chooser weight
    public float MinRange;
    public float MaxRange;
    public bool los;

    [Tooltip("Is a percentage value of the enemy's base damage")]
    public float Damage;
    public float Knockback;
    public bool Blockable;
    public bool Paryable;
    public bool onHitCancel;
    public List<EnemyVFXs> VFXs;

    public float Cooldown;

    [Header("Animations")]
    public int Animation_Index;
    public bool Rootmotion;
}
