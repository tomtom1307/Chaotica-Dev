using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StatUpgradeUI : MonoBehaviour
{
    //UI gameobject
    public GameObject Canvas;

    [HideInInspector]public Interactable _inter;
    [HideInInspector]public PlayerInventory PlayerInventory;

    public TMP_Text ChaosCoresTxt;

    //Function Called by interactable 
    public void _EnterUpgradeWindow()
    {
        UpdateUIValues();
        UIManager.instance.IsMenu(true, Canvas);
        UIManager.instance.ShowCursor();
        

    }

    List<StatUpgradeContainer> StatUpgrades;

    private void Start()
    {
        StatUpgrades = GetComponentsInChildren<StatUpgradeContainer>(true).ToList();
        _inter = GetComponent<Interactable>();
    }

    

    public void UpdateUIValues()
    {
        PlayerInventory = _inter.interactor.GetComponent<PlayerInventory>();
        GetCurrencyValue();
        foreach (var item in StatUpgrades)
        {
            item.UpgradeStore = this;
            item.Initialize();
        }
    }

    public void GetCurrencyValue()
    {
        ChaosCoresTxt.text = $"CC: {_inter.interactor.GetComponent<PlayerInventory>().ChaosCores}";
    }









}
