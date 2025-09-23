using System;
using System.Collections.Generic;
using UnityEngine;


public class EAI_AttackSO_Base : EAI_AbilitySO
{
    public bool AimAtTarget;
    public float AimSpeed;
    public LayerMask RaycastAttackHit;
    public override void Enter(EnemyContext EC)
    {
        
    }

    public override void Execute(EnemyContext EC)
    {
        
    }

    public override void Exit(EnemyContext EC)
    {
        EC.aimer.ResetSpeedToDefault();
    }

    public void ApplyDamage(PlayerHealth player, float Damage)
    {
        player.TakeDamage(Damage);
    }

    public override void Tick(EnemyContext EC)
    {
        if (AimAtTarget) { EC.aimer.AimAt(EC.bb.target.position, AimSpeed); }
    }

    public override void LateTick(EnemyContext EC)
    {
        
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