using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;


public class Ability : ScriptableObject
{
    public new string name;
    public float activeTime;
    public float cooldownTime;

    public virtual void Activate(GameObject parent)
    {
        //yasbbab
    } 
}
