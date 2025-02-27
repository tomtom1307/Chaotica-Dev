using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyBehaviour
{
    public Conditions conditions;
    public List<B_Action> actionList;
    

    public void DoActions()
    {
        //e.g. chase player during attack state: move to player position
    }
    public void DoAttacks()
    {

    }
}
