using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    public Weapon_Input weaponInput1;
    public Weapon_Input weaponInput2;
    public Weapon_Input weaponInput3;

    public AnimatorController Anim_controller;

    [SerializeReference] // This enables polymorphic serialization
    public List<Weapon_Attack_Data_Base> Weapon_Attacks = new List<Weapon_Attack_Data_Base>();






}
