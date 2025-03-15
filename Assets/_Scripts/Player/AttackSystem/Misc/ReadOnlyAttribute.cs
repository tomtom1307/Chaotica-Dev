using UnityEngine;
using UnityEditor;

public class ReadOnlyyAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(ReadOnlyyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true;
    }
}

