using System;
using UnityEngine;

[Serializable]
public class Attack : B_Action
{
    [SerializeField] public int attackType;
    public Attack() { B_action = b_Action.Attack; }
}
