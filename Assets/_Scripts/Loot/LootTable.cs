using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LootTable", menuName = "LootTable")]
public class LootTable : ScriptableObject
{
    public List<LootTableElement> lootTableElements;
}

[Serializable]
public struct LootTableElement
{
    public string LootName;
    public List<LootItem> LootItems;
}


[Serializable]
public struct LootItem
{
    public GameObject gameObject;
    public int Probability;
    public float amount;
}

