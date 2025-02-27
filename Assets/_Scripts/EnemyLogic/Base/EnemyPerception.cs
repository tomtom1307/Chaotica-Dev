using System.Collections.Generic;
using UnityEngine;

public class EnemyPerception : MonoBehaviour
{
    public bool LOS;
    public void UpdateFrameVariables(List<string> functions)
    {
        foreach (string func in functions) Invoke(func,0);
    }
    public void StartRepeatingChecks(List<string> functions, List<float> times)
    {
        for (int i = 0; i < functions.Count; i++)
        {
            InvokeRepeating(functions[i], 0, times[i]);
        }
    }
    public void StopRepeatingChecks() => CancelInvoke();
    public bool CheckLOS() { return false; }
    public float CheckPlayerDistance() { return 0f; }
    public int CountEnemies() { return 0; }
}
