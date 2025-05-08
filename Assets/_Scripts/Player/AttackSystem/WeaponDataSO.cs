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
    
    public Sprite InventorySprite;
    

    public enum Hand
    {
        left,
        right
    }

}
