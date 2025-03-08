using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAttack
{
    [SerializeField] public EnemyAttackSO attackData;
    [SerializeField] public List<colliderGroup> colliderGroups;
    [SerializeField] public int attackType;

}

[Serializable]
public struct colliderGroup
{
    public ColliderDetector collider;
    public float DetectTime;
}
