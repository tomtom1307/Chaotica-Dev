using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAttack
{
    [SerializeField] public EnemyAttackSO attackData;
    [SerializeField] public List<colliderGroup> colliderGroups;
    [SerializeField] public int attackAnimation;
    [SerializeField] public List<EnemyVFXs> VFXs;

}

