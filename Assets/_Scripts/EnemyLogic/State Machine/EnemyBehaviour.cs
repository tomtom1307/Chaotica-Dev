using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyBehaviour
{
    public Conditions conditions;
    public List<B_Action> actionList;
    public (List<CheckBool> boolCheck, List<CheckFloat> floatCheck, List<CheckInt> intCheck) TimeChecks;
    public (List<CheckBool> boolCheck, List<CheckFloat> floatCheck, List<CheckInt> intCheck) FrameChecks;

    public void InitializeBehaviour(EnemyBrain brain)
    {
        foreach (var action in actionList)
        {
            action.brain = brain;
        }
        conditions.brain = brain;
    }

    public void SetCheckTypes()
    {
        conditions.InitialCheck(out FrameChecks,out TimeChecks);
    }



    public void DoActions()
    {
        
    }
    public void DoAttacks()
    {

    }

}
