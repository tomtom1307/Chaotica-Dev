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
                return "LOS"; // replace "LOS" with actual LOSchecking function name
            default:
                Debug.LogError("You are trying to invoke a time>0 check on a variable for which this has not been implemented" + checkBool);
                break;
        }
        return "";
    }

}
