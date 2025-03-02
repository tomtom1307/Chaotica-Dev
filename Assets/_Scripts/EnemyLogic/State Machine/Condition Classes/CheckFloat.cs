using System;
using UnityEngine;

[Serializable]
public class CheckFloat
{
    public float checkTime = 0.1f;
    public Float checkFloat;
    public Comparison checkComparison;
    public enum Float 
    {
        playerDistance, 
        LSP_time,
        DetectionMeter, 
        CurrentHealth
    }
    public enum Comparison {greaterThan, lessThan, greaterThanOrEqualTo, lessThanOrEqualTo}
    public float input;
    public (Float checkFloat, Comparison checkComparison, float input) ReturnCondition() => (checkFloat, checkComparison, input);

    public string ReturnFunctionString()
    {
        switch (checkFloat)
        {
            case Float.playerDistance:
                return "CheckPlayerDistance";
            case Float.LSP_time:
                return "CheckLastSeenPlayerTime";
            case Float.DetectionMeter:
                return "CheckDetectionMeter";
            default:
                Debug.LogError("You are trying to invoke a function name on a variable for which this has not been implemented" + checkFloat);
                break;
        }
        return "";
    }
}
