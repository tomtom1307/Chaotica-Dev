using System;
using System.Collections.Generic;
using UnityEngine;


public class EAI_AttackSO_Base : EAI_AbilitySO
{
    public float Damage;
    public bool Blockable;
    public bool Paryable;
    public List<EnemyVFXs> VFXs;

    public override void Enter(GameObject owner, in AbilityContext ctx)
    {

    }

    public override void Execute(GameObject owner, in AbilityContext ctx)
    {

    }

    public override void Exit(GameObject owner, in AbilityContext ctx)
    {

    }

    public void ApplyDamage(PlayerHealth player, float Damage)
    {
        player.TakeDamage(Damage);
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