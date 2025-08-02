using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDistribution", menuName = "Enemy Distribution")]
public class EnemyDistributionBase : ScriptableObject
{
    public List<EnemyDistEntry> EnemyDistribution;
}

[Serializable]
public struct EnemyDistEntry
{
    public string name;
    public GameObject EnemyPrefab;
    public float prob;
}
