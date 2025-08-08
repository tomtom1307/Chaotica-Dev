using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "ChainLightning", menuName = "Abilities/ChainLightning")]
public class ChainLightning : Ability
{
    public float DPS = 3;
    public int EnemyNumber;
    public float JumpRange = 10;
    public float MaxViewAngle = 15;
    [SerializeField] GameObject LightningPrefab;

    List<GameObject> spawnedLRs = new List<GameObject>();

    public override void Activate(GameObject parent, AbilityHolder holder)
    {
        base.Activate(parent, holder);
        GameObject target = GetBestEnemy(JumpRange, MaxViewAngle, parent);
        if(target != null)
        {
            StartShooting(parent,target);
        }

    }

    public override void AbilityUpdate(GameObject parent, AbilityHolder holder)
    {
        base.AbilityUpdate(parent, holder);
    }

    public override void Deactivate(GameObject parent, AbilityHolder holder)
    {
        base.Deactivate(parent, holder);
        StopShooting();
    }




    public void StartShooting(GameObject parent, GameObject target)
    {
        if(LightningPrefab != null)
        {
            NewLineRenderer(parent.transform, target.transform);
        }
    }

    public void StopShooting()
    {
        for(int i = 0; i < spawnedLRs.Count; i++)
        {
            Destroy(spawnedLRs[i]);
        }
        spawnedLRs.Clear();
    }

    public void NewLineRenderer(Transform startPos, Transform endPos)
    {
        GameObject lineRend = Instantiate(LightningPrefab);
        Debug.Log(lineRend);
        spawnedLRs.Add(lineRend);
        holder.StartCoroutineUpdateLineRenderer(lineRend, startPos, endPos);
    }

}



