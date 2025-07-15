using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using static WeaponDataSO;

[CreateAssetMenu(menuName = "WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    public string _Name;

    public GameObject model;
    public GameObject secondaryModel;
    public float WeaponDamage;
    public AnimatorController Anim_controller;
    public Hand hand;
    [SerializeReference] // This enables polymorphic serialization
    public List<Weapon_Attack_Data_Base> Weapon_Attacks = new List<Weapon_Attack_Data_Base>();
    public Rarity rarity;
    public Sprite InventorySprite;
    public Vector3 DroppedWeaponSize;
    public Vector3 DroppedWeaponQuaternion;

    public enum Hand
    {
        left,
        right,
        Twohanded
    }


    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic,
        Random // This is for weapon generation DONT ASSIGN TO WEAPONS
    }

    public static Color GetColorByRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Color.gray;
            case Rarity.Uncommon: return Color.green;
            case Rarity.Rare: return Color.blue;
            case Rarity.Epic: return new Color(0.6f, 0f, 1f); // Purple
            case Rarity.Legendary: return new Color(1f, 0.5f, 0f); // Orange
            case Rarity.Mythic: return Color.red;
            default: return Color.white;
        }
    }

    public RarityDistribution rarityDistribution;

}



[Serializable]
public struct RarityWeight
{
    public Rarity rarity;
    [Tooltip("Relative probability weight for this rarity")]
    public float weight;
}

[Serializable]
public class RarityDistribution
{
    [Tooltip("Set a weight for each Rarity; higher = more common.")]
    public List<RarityWeight> weights = new List<RarityWeight>();


    private void OnValidate()
    {
        var allRarityValues = Enum.GetValues(typeof(WeaponDataSO.Rarity));
        foreach (WeaponDataSO.Rarity r in allRarityValues)
        {
            if (!weights.Exists(w => w.rarity == r))
                weights.Add(new RarityWeight { rarity = r, weight = 0f });
        }
        weights.Sort((a, b) => ((int)a.rarity).CompareTo((int)b.rarity));
    }

    /// <summary>
    /// Pick a rarity based on the weights you assigned in the inspector.
    /// </summary>
    public Rarity RollRarity()
    {
        float total = 0;
        for (int i = 0; i < weights.Count; i++)
            total += weights[i].weight;

        float rnd = UnityEngine.Random.value * total;
        float accum = 0;
        foreach (var entry in weights)
        {
            accum += entry.weight;
            if (rnd <= accum)
                return entry.rarity;
        }
        // fallback (shouldn’t happen if total>0)
        return Rarity.Common;
    }
}



