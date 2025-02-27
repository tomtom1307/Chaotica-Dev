using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Conditions
{
    EnemyBrain brain;
    public List<CheckBool> boolChecks;
    public List<CheckFloat> floatChecks;
    public List<CheckInt> intChecks;
    public bool CheckConditions()
    {
        (CheckFloat.Float checkFloat, CheckFloat.Comparison checkComparison, float input) floatTupleCondition;
        (CheckInt.Int checkInt, CheckInt.Comparison checkComparison, float input) intTupleCondition;
        bool condition = true;
        foreach (CheckBool check in boolChecks)
        {
            condition = condition && brain.conditionBools[check.ReturnCondition()];
        }
        if (!condition) return false;
        foreach (CheckFloat check in floatChecks)
        {
            floatTupleCondition = check.ReturnCondition();
            condition = condition && DoFloatCheck(brain.conditionFloats[floatTupleCondition.checkFloat], floatTupleCondition.checkComparison, floatTupleCondition.input);
        }
        if (!condition) return false;
        foreach (CheckInt check in intChecks)
        {
            intTupleCondition = check.ReturnCondition();
            condition = condition && DoIntCheck(brain.conditionInts[intTupleCondition.checkInt], intTupleCondition.checkComparison, intTupleCondition.input);
        }

        return condition;
    }

    public List<string> InitialCheck()
    {
        List<CheckBool> pseudoBoolChecks = boolChecks;
        List<CheckFloat> pseudoFloatChecks = floatChecks;
        List<CheckInt> pseudoIntChecks = intChecks;

        foreach (CheckBool check in pseudoBoolChecks)
        {
            if(check.checkTime <= 0);
        }
        return new List<string> { "lol" };
    }

    private bool DoFloatCheck(float value, CheckFloat.Comparison comparison, float input) { return false; } // do switch case for different comparison methods 
    private bool DoIntCheck(float value, CheckInt.Comparison comparison, float input) { return false; } // do switch case for different comparison methods 

}
