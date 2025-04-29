using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeTome : MonoBehaviour
{
    public GameObject canvas;
    public List<UpgradeTomePool> _Upool;
    List<TomeUpgradePannel> _Upannels;
    public bool Generated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject player;
    public void Interact()
    {
        UIManager.instance.IsMenu(true , canvas);
        UIManager.instance.ShowCursor();
        player = GetComponent<Interactable>().interactor;
        GenerateUpgrades();
    }


    public void GenerateUpgrades()
    {
        if (Generated) { return; }
        Generated = true;

        _Upannels = GetComponentsInChildren<TomeUpgradePannel>(true).ToList();
        List<UpgradeTomePool> UpgradePool = new List<UpgradeTomePool>(_Upool); // Make a copy to avoid modifying the original list

        foreach (var pannel in _Upannels)
        {
            UpgradeTomePool selectedUpgrade = GenerateUpgrade(UpgradePool);
            UpgradePool.Remove(selectedUpgrade); // Remove from the pool so it's not selected again
            pannel.PopulateInfo(selectedUpgrade.Upgrade, this);
        }
    }

    public UpgradeTomePool GenerateUpgrade(List<UpgradeTomePool> pool)
    {
        if (pool.Count == 0) 
        { 
            UpgradeTomePool noneType = new UpgradeTomePool();
            noneType.weight = 0;
            return noneType; 
        } // Safety check

        int maxWeight = pool.Sum(item => item.weight);
        int randomValue = UnityEngine.Random.Range(0, maxWeight);

        foreach (var item in pool)
        {
            if (randomValue < item.weight)
            {
                return item;
            }
            randomValue -= item.weight;
        }

        return pool.Last(); // Fallback in case of rounding errors
    }



}
[Serializable]
public struct UpgradeTomePool
{   
    public TomeUpgradeSOBase Upgrade;
    public int weight;
}
