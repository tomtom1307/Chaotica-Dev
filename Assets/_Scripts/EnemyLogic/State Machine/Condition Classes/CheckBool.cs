using System;
using UnityEngine;

[Serializable]
public class CheckBool
{
    public float checkTime;
    public Bool checkBool;

    public enum Bool {isAlert, LOS}

    public Bool ReturnCondition() => checkBool;

    public string ReturnFunctionString()
    {
        switch (checkBool)
        {
            case Bool.LOS:
                return "CheckLOS";
            default:
                Debug.LogError("You are trying to invoke a function name on a variable for which this has not been implemented" + checkBool);
                break;
        }
        return "";
    }

}
