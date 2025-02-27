using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

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
        (CheckInt.Int checkInt, CheckInt.Comparison checkComparison, int input) intTupleCondition;
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

    public ((List<CheckBool> frameBool, List<CheckFloat> frameFloat, List<CheckInt> frameInt),(List<CheckBool> timeBool, List<CheckFloat> timeFloat, List<CheckInt> timeInt)) InitialCheck()
    {
        List<CheckBool> frameBoolChecks = new List<CheckBool>();
        List<CheckFloat> frameFloatChecks = new List<CheckFloat>();
        List<CheckInt> frameIntChecks = new List<CheckInt>();
        List<CheckBool> timeBoolChecks = new List<CheckBool>();
        List<CheckFloat> timeFloatChecks = new List<CheckFloat>();
        List<CheckInt> timeIntChecks = new List<CheckInt>();

        foreach (CheckBool check in boolChecks)
        {
            if (check.checkTime <= 0) frameBoolChecks.Add(check); else timeBoolChecks.Add(check);
        }
        foreach (CheckFloat check in floatChecks)
        {
            if (check.checkTime <= 0) frameFloatChecks.Add(check); else timeFloatChecks.Add(check);
        }
        foreach (CheckInt check in intChecks)
        {
            if (check.checkTime <= 0) frameIntChecks.Add(check); else timeIntChecks.Add(check);
        } //.Select(x => x.ReturnFunctionString()).ToList()
        return ((frameBoolChecks, frameFloatChecks, frameIntChecks), (timeBoolChecks, timeFloatChecks, timeIntChecks));
    }

    private bool DoFloatCheck(float value, CheckFloat.Comparison comparison, float input) 
    {
        switch (comparison)
        {
            case CheckFloat.Comparison.greaterThan:
                return value > input;
            case CheckFloat.Comparison.lessThan:
                return value < input;
            case CheckFloat.Comparison.greaterThanOrEqualTo:
                return value >= input;
            case CheckFloat.Comparison.lessThanOrEqualTo:
                return value <= input;
            default:
                break;
        }
        Debug.LogError("non-existant comparison in enemy behaviour float condition. Returning false comparison.");
        return false;
    }
    private bool DoIntCheck(int value, CheckInt.Comparison comparison, int input)
    {
        switch (comparison)
        {
            case CheckInt.Comparison.equalTo:
                return value == input;
            case CheckInt.Comparison.greaterThan:
                return value > input;
            case CheckInt.Comparison.lessThan:
                return value < input;
            case CheckInt.Comparison.greaterThanOrEqualTo:
                return value >= input;
            case CheckInt.Comparison.lessThanOrEqualTo:
                return value <= input;
            default:
                break;
        }
        Debug.LogError("non-existant comparison in enemy behaviour integer condition. Returning false comparison.");
        return false;
    }
}