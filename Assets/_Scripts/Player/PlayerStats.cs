using System;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

// Define Enum of all stats
public enum StatType
{
    MaxHealth, MaxMana, MoveSpeed,
    MaxHealthIncrease, ManaIncrease, MoveSpeedIncrease,
    DamageResistance, AllDamageBuff, UmbravailDamageBuff, ScarForgeDamageBuff,
    VerdancyDamageBuff, AetherflowDamageBuff, CritChance,
    ArcheryLevel, ArcheryXP, ArcheryLevelUpAmount,
    SwordsmanshipLevel, SwordsmanshipXP, SwordsmanshipLevelUpAmount,
    WizardryLevel, WizardryXP, WizardryLevelUpAmount
}



public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    //For Proficiencies
    public AnimationCurve progressionCurve;

    [SerializedDictionary("Stat","Value")]
    public SerializedDictionary<StatType, float> stats;

    void Awake()
    {
        InitializeStats();
        if(instance == null)
        {
            instance = this;
        }
    }

    //Initialize Dictionary
    private void InitializeStats()
    {
        stats[StatType.MaxHealth] = 100f;
        stats[StatType.MaxMana] = 50f;
        stats[StatType.MoveSpeed] = 5f;

        stats[StatType.MaxHealthIncrease] = 0f;
        stats[StatType.ManaIncrease] = 0f;
        stats[StatType.MoveSpeedIncrease] = 0f;

        stats[StatType.DamageResistance] = 0f;
        stats[StatType.AllDamageBuff] = 100f;
        stats[StatType.UmbravailDamageBuff] = 0f;
        stats[StatType.ScarForgeDamageBuff] = 0f;
        stats[StatType.VerdancyDamageBuff] = 0f;
        stats[StatType.AetherflowDamageBuff] = 0f;
        stats[StatType.CritChance] = 0f;

        stats[StatType.ArcheryLevel] = 1;
        stats[StatType.ArcheryXP] = 0f;
        stats[StatType.ArcheryLevelUpAmount] = 100f;

        stats[StatType.SwordsmanshipLevel] = 1;
        stats[StatType.SwordsmanshipXP] = 0f;
        stats[StatType.SwordsmanshipLevelUpAmount] = 100f;

        stats[StatType.WizardryLevel] = 1;
        stats[StatType.WizardryXP] = 0f;
        stats[StatType.WizardryLevelUpAmount] = 100f;
    }

    //Modify value
    public void ModifyStat(StatType stat, float value)
    {
        if (stats.ContainsKey(stat))
        {
            stats[stat] += value;
        }
        else
        {
            Debug.LogWarning($"Stat '{stat}' not found.");
        }
    }

    //Get value
    public float GetStat(StatType stat)
    {
        return stats.ContainsKey(stat) ? stats[stat] : 0f;
    }

    //Set value
    public void SetStat(StatType stat, float value)
    {
        if (stats.ContainsKey(stat))
        {
            stats[stat] = value;
        }
        else
        {
            Debug.LogWarning($"Stat '{stat}' not found.");
        }
    }
}
