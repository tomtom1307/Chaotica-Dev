using System;
using UnityEngine;

[Serializable]
public class CheckInt
{
    public float checkTime;
    public Int checkInt;
    public Comparison checkComparison;
    public enum Int { numberOfEnemies }
    public enum Comparison { equalTo, greaterThan, lessThan, greaterThanOrEqualTo, lessThanOrEqualTo }

    public int input;
    public (Int checkFloat, Comparison checkComparison, int input) ReturnCondition() => (checkInt, checkComparison, input);

    public string ReturnFunctionString()
    {
        switch (checkInt)
        {
            case Int.numberOfEnemies:
                return "CountEnemies";
            default:
                Debug.LogError("You are trying to invoke a function name on a variable for which this has not been implemented" + checkInt);
                break;
        }
        return "";
    }
}
