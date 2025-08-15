// Assets/Editor/WeaponDataSOEditor.cs
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(WeaponDataSO))]
public class WeaponDataSOEditor : Editor
{
    SerializedProperty defaultModifiersProp;
    SerializedProperty weaponAttacksProp;

    PolyList modifiersList;
    PolyList attacksList;

    void OnEnable()
    {
        defaultModifiersProp = serializedObject.FindProperty("defaultModifiers");
        weaponAttacksProp = serializedObject.FindProperty("Weapon_Attacks");

        // Build two polymorphic lists with their own type menus
        modifiersList = new PolyList(
            serializedObject,
            defaultModifiersProp,
            header: "Default Modifiers",
            baseType: typeof(WeaponModifier)
        );

        attacksList = new PolyList(
            serializedObject,
            weaponAttacksProp,
            header: "Weapon Attacks",
            baseType: typeof(Weapon_Attack_Data_Base)
        );
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw everything else except the two managed lists
        DrawPropertiesExcluding(serializedObject, "m_Script", "defaultModifiers", "Weapon_Attacks");

        EditorGUILayout.Space(8);
        modifiersList.DoLayoutList();

        EditorGUILayout.Space(8);
        attacksList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    // -------- Helper class to avoid duplicating list logic --------
    class PolyList
    {
        readonly SerializedObject so;
        readonly SerializedProperty arrayProp;
        readonly string header;

        readonly ReorderableList list;
        readonly List<Type> types;
        readonly string[] typeNames;

        public PolyList(SerializedObject so, SerializedProperty arrayProp, string header, Type baseType)
        {
            this.so = so;
            this.arrayProp = arrayProp;
            this.header = header;

            // Find all non-abstract, non-generic subclasses with a parameterless ctor (public OR non-public)
            types = TypeCache.GetTypesDerivedFrom(baseType)
                .Where(t =>
                    !t.IsAbstract &&
                    !t.IsGenericType &&
                    t.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                     binder: null, types: Type.EmptyTypes, modifiers: null) != null)
                .OrderBy(t => t.Name)
                .ToList();

            typeNames = types.Select(t => Nicify(t.Name)).ToArray();

            list = new ReorderableList(so, arrayProp, draggable: true, displayHeader: true, displayAddButton: true, displayRemoveButton: false);

            list.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, header, EditorStyles.boldLabel);
            };

            list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = arrayProp.GetArrayElementAtIndex(index);
                rect.y += 2;

                // Header line with type + Remove button
                var headerRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
                var btnRect = new Rect(headerRect.xMax - 70, headerRect.y, 70, headerRect.height);
                var typeName = GetManagedTypeName(element) ?? "(null)";
                EditorGUI.LabelField(headerRect, typeName, EditorStyles.boldLabel);
                if (GUI.Button(btnRect, "Remove"))
                {
                    arrayProp.DeleteArrayElementAtIndex(index);
                    return;
                }

                // Draw full property below the header
                var propHeight = EditorGUI.GetPropertyHeight(element, includeChildren: true);
                var propRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 4, rect.width, propHeight);
                EditorGUI.PropertyField(propRect, element, includeChildren: true);
            };

            list.elementHeightCallback = index =>
            {
                var element = arrayProp.GetArrayElementAtIndex(index);
                var h = EditorGUIUtility.singleLineHeight + 4; // header
                if (element != null)
                    h += EditorGUI.GetPropertyHeight(element, true);
                return h + 6; // bottom padding
            };

            list.onAddDropdownCallback = (rect, l) =>
            {
                var menu = new GenericMenu();

                if (types.Count == 0)
                {
                    menu.AddDisabledItem(new GUIContent($"No {baseType.Name} types found"));
                }
                else
                {
                    foreach (var t in types)
                    {
                        var captured = t;
                        menu.AddItem(new GUIContent(Nicify(t.Name)), false, () =>
                        {
                            so.Update();
                            int newIndex = arrayProp.arraySize;
                            arrayProp.InsertArrayElementAtIndex(newIndex);
                            var element = arrayProp.GetArrayElementAtIndex(newIndex);
                            // Create instance using non-public allowed
                            element.managedReferenceValue = Activator.CreateInstance(captured, /*nonPublic*/ true);
                            so.ApplyModifiedProperties();
                        });
                    }
                }

                menu.DropDown(rect);
            };

            list.onRemoveCallback = l =>
            {
                if (l.index >= 0 && l.index < arrayProp.arraySize)
                    arrayProp.DeleteArrayElementAtIndex(l.index);
            };
        }

        public void DoLayoutList() => list.DoLayoutList();

        static string GetManagedTypeName(SerializedProperty prop)
        {
            var full = prop?.managedReferenceFullTypename;
            if (string.IsNullOrEmpty(full)) return null;
            var lastSpace = full.LastIndexOf(' ');
            var typeName = lastSpace >= 0 ? full[(lastSpace + 1)..] : full;
            return Nicify(typeName.Split('.').Last());
        }

        static string Nicify(string s)
        {
            var spaced = System.Text.RegularExpressions.Regex.Replace(s, "(\\B[A-Z])", " $1");
            return ObjectNames.NicifyVariableName(spaced);
        }
    }
}
#endif
