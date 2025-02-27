using System;
using UnityEngine;

[Serializable]
public class CheckInt
{
    public float checkTime;
    public Int checkInt;
    public Comparison checkComparison;
    public enum Int { numberOfEnemies }
    public enum Comparison { }

    public int input;
    public (Int checkFloat, Comparison checkComparison, float input) ReturnCondition() => (checkInt, checkComparison, input);

    public string ReturnFunctionString()
    {
        switch (checkInt)
        {
            default:
                Debug.LogError("You are trying to invoke a time>0 check on a variable for which this has not been implemented" + checkInt);
                break;
        }
        return "";
    }
}
