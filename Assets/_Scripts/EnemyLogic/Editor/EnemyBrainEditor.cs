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
        List<B_Action> newActionList;
        foreach (EnemyState state in brain.stateList)
        {
            foreach(EnemyBehaviour behaviour in state.enemyBehaviours)
            {
                newActionList = behaviour.actionList;
                for (int i = 0; i < behaviour.actionList.Count; i++)
                {
                    switch (behaviour.actionList[i].B_action)
                    {
                        case B_Action.b_Action.AttackSet:
                            newActionList[i] = new Attack();
                            Debug.Log("Assigned attack instance to list.");
                            break;
                        default:
                            break;
                    }
                }
                behaviour.actionList = newActionList;
            }    
        }
        base.OnInspectorGUI(); // Draw default inspector
    }
}
