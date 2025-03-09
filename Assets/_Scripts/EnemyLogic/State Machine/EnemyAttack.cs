using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAttack
{
    [SerializeField] public EnemyAttackSO attackData;
    [SerializeField] public List<colliderGroup> colliderGroups;
    [SerializeField] public int attackAnimation;

}
[Serializable]
public struct colliderGroup
{
    [SerializeField] private string Name;
    [SerializeField] public List<ColliderDetector> colliderList;
}
