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
                Debug.Log(nameof(EnemyPerception.CheckLOS) + ": Returned LOS");
                return nameof(EnemyPerception.CheckLOS);
            default:
                Debug.LogError(checkBool+" :You are trying to invoke a function name on a variable for which this has not been implemented");
                break;
        }
        Debug.LogError("Somehow the checkBool had no defined string");
        return "";
    }

}
