using System;
using UnityEngine;

[Serializable]
public class CheckFloat
{
    public float checkTime;
    public Float checkFloat;
    public Comparison checkComparison;
    public enum Float {playerDistance}
    public enum Comparison {greaterThan, lessThan, greaterThanOrEqualTo, lessThanOrEqualTo}
    public float input;
    public (Float checkFloat, Comparison checkComparison, float input) ReturnCondition() => (checkFloat, checkComparison, input);

    public string ReturnFunctionString()
    {
        switch (checkFloat)
        {
            case Float.playerDistance:
                return "fname"; // replace "LOS" with actual distancechecking function name
            default:
                Debug.LogError("You are trying to invoke a time>0 check on a variable for which this has not been implemented" + checkFloat);
                break;
        }
        return "";
    }
}
