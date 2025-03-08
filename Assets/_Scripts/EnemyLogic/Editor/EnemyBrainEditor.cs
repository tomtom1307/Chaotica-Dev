using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(EnemyBrain))]
public class EnemyBrainEditor : UnityEditor.Editor
{
    private EnemyBrain brain;
    private void OnEnable()
    {
        brain = target as EnemyBrain;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Draw default inspector
    }
}
