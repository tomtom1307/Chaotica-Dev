using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponDataSO))]
public class WeaponEditor : Editor
{
    private SerializedProperty weaponAttacksProperty;

    private void OnEnable()
    {
        // Cache the serialized property for the list
        weaponAttacksProperty = serializedObject.FindProperty("Weapon_Attacks");
    }

    public override void OnInspectorGUI()
    {
        WeaponDataSO SO = (WeaponDataSO)target;

        serializedObject.Update(); // Ensure the serialized object is up to date

        DrawDefaultInspector();

        if (GUILayout.Button("Add Melee Attack"))
        {
            AddNewAttack(AttackType.Melee, SO);
        }
        if (GUILayout.Button("Add AOE Attack"))
        {
            AddNewAttack(AttackType.AOE, SO);
        }
        if (GUILayout.Button("Add Projectile Attack"))
        {
            AddNewAttack(AttackType.Projectile, SO);
        }
        if (GUILayout.Button("Add Raycast Attack"))
        {
            AddNewAttack(AttackType.Raycast, SO);
        }
        

        serializedObject.ApplyModifiedProperties(); // Save any modified properties
    }

    private void AddNewAttack(AttackType attackType, WeaponDataSO SO)
    {
        serializedObject.Update();

        if (weaponAttacksProperty == null)
        {
            Debug.LogError("Weapon_Attacks property not found!");
            return;
        }

        Weapon_Attack_Data_Base newAttack = null;

        switch (attackType)
        {
            case AttackType.Melee:
                newAttack = new Weapon_Attack_Data_Melee();
                break;
            case AttackType.Projectile:
                newAttack = new Weapon_Attack_Data_Projectile();
                break;
            case AttackType.AOE:
                newAttack = new Weapon_Attack_Data_AOE();
                break;
            case AttackType.Raycast:
                //newAttack = new Weapon_Attack_Data_Raycast();
                break;
        }

        if (newAttack != null)
        {
            SO.Weapon_Attacks.Add(newAttack);
            EditorUtility.SetDirty(SO);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
