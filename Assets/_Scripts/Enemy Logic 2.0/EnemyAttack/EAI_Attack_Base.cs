using System;
using System.Collections.Generic;
using UnityEngine;


public class EAI_AttackSO_Base : EAI_AbilitySO
{
    [Tooltip("Is a percentage value of the enemy's base damage")]
    public float Damage = 100;
    public float Knockback = 100;
    public bool Blockable;
    public bool Paryable;
    public bool AimAtTarget;
    public float AimSpeed = 100;
    public bool onHitCancel;
    public List<EnemyVFXs> VFXs;

    public override void Enter(EnemyContext EC, in AbilityContext ctx)
    {
        
    }

    public override void Execute(EnemyContext EC, in AbilityContext ctx)
    {

    }

    public override void Exit(EnemyContext EC, in AbilityContext ctx)
    {
        EC.aimer.ResetSpeedToDefault();
    }

    public void ApplyDamage(PlayerHealth player, float Damage)
    {
        player.TakeDamage(Damage);
    }

    public override void Tick(EnemyContext EC, in AbilityContext A_ctx)
    {
        if (AimAtTarget) { EC.aimer.AimAt(EC.bb.target.position, AimSpeed); }
    }
}

[Serializable]
public struct colliderGroup
{
    [SerializeField] private string Name;
    [SerializeField] public List<ColliderDetector> colliderList;
}

[Serializable]
public struct EnemyVFXs
{
    [SerializeField] public string Name;
    [SerializeField] public GameObject Prefab;
    [SerializeField] public bool isParentedToHolder;
}