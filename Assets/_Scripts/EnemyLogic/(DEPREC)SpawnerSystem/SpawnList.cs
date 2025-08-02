using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnList", menuName = "SpawnList")]
public class SpawnList : ScriptableObject
{
    public List<SpawnInfo> spawnList;
}
[Serializable]
public struct SpawnInfo
{
    public string Name;
    public GameObject Enemy;
    public float amount;
}