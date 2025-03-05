using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    public string _Name;

    public float WeaponDamage;
    public AnimatorController Anim_controller;

    [SerializeReference] // This enables polymorphic serialization
    public List<Weapon_Attack_Data_Base> Weapon_Attacks = new List<Weapon_Attack_Data_Base>();

    public Sprite InventorySprite;




}
