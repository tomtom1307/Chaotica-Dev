using UnityEditor;
using UnityEngine;


public class RandomizeAttribute : PropertyAttribute
{
    public readonly float minVal;
    public readonly float maxVal;

    public RandomizeAttribute(float minVal, float maxVal)
    {
        this.minVal = minVal;
        this.maxVal = maxVal;
    }



}


