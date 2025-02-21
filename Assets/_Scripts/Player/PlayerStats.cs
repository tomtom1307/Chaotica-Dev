using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Standard Stats")]
    public float MaxHealth;
    public float MaxMana;
    public float MoveSpeed;

    [Header("Modifiers")]
    public float MaxHealthIncrease;
    public float WizardryIncrease;
    public float MoveSpeedIncrease;


    [Header("Combat Stats")]
    public float DamageResistance;
    public float AllDamageBuff;
    public float UmbravailDamageBuff;
    public float ScarForgeDamageBuff;
    public float Verdancy;
    public float Aetherflow;
    public float CritChance;

    [Header("Proficiencies")]
    public AnimationCurve progressionCurve;

    [Header("Archery")]
    public int ArcheryLevel;
    public float ArcheryXP;
    public float ArcheryLevelUpAmount;

    [Header("Swordsmanship")]
    public int SwordsmanshipLevel;
    public float SwordsmanshipXP;
    public float SwordsmanshipLevelUpAmount;

    [Header("Wizardry")]
    public int WizardryLevel;
    public float WizardryXP;
    public float WizardryLevelUpAmount;



}
