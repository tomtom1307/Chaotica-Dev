using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LootPackage : MonoBehaviour
{
    public LootTable LootTable;
    public float ForceMin;
    public float ForceMax;
    public Vector3 ForceDir1;
    public Vector3 ForceDir2;
    void Start()
    {
        GenerateLoot(LootTable);
        Destroy(gameObject, 5);
    }

    public void GenerateLoot(LootTable LT)
    {
        foreach (var element in LT.lootTableElements)
        {
            HandleLootTableElement(element.LootItems);
        }
    }


    public void HandleLootTableElement(List<LootItem> lootItems)
    {
        float MaxProb = FindMaxProbability(lootItems);
        float RNG = UnityEngine.Random.Range(0, MaxProb);
        LootItem LI = EvaluateRNG(lootItems, RNG, MaxProb);
        if (LI.gameObject == null) return;
        for (int i = 0; i < LI.amount; i++)
        {
            
            GameObject Obj = Instantiate(LI.gameObject, transform.position, Quaternion.identity);
            Rigidbody rb = Obj.GetComponent<Rigidbody>();
            Vector3 dirVec = GenerateRandomDir();
            float ForceMag = UnityEngine.Random.Range(ForceMin, ForceMax);
            rb.AddForce(ForceMag * dirVec);
        }
        
        

    }

    public LootItem EvaluateRNG(List<LootItem> lootItems, float RNG, float MaxProb)
    {
        LootItem lootitem = lootItems[0];
        float RarestProb = MaxProb;
        foreach (var _item in lootItems)
        {
            if(_item.Probability < RarestProb && _item.Probability >= RNG)
            {
                RarestProb = _item.Probability;
                lootitem = _item;
            }
        }

        return lootitem;
    }

    public float FindMaxProbability(List<LootItem> lootItems)
    {
        float x = 0;
        foreach (var item in lootItems)
        {
            if(x < item.Probability)
            {
                x = item.Probability;
            }
        }
        return x;
    }

    public Vector3 GenerateRandomDir()
    {
        return TRTools.VecOp.RandomDir(ForceDir1, ForceDir2);
    }



}
