using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

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
        Mythic
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

}
