using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AbilitySystemUI : MonoBehaviour
{
    //UI gameobject
    public GameObject Canvas;
    public GameObject Player;

    //private AbilityUIController AbilityUIController;

    [HideInInspector] public Interactable _inter;
    [HideInInspector] public PlayerInventory PlayerInventory;

    public TMP_Text ChaosCoresTxt;
    public TreeNode cursel;

    // reference to abilityholders

    public List <AbilityHolder> AHolder;

    // reference to abilityslots

    

    //Function Called by interactable 
    public void _EnterAbilityWindow()
    {
        //UpdateUIValues();
        UIManager.instance.IsMenu(true, Canvas);
        UIManager.instance.ShowCursor();

        // Debug.Log(AbilityUIController);

        Player = _inter.interactor;

        AHolder = Player.GetComponents<AbilityHolder>().ToList();
    }

    //List<StatUpgradeContainer> StatUpgrades;

    private void Start()
    {
        _inter = GetComponent<Interactable>();

        
       // AbilityUIController = GetComponentInChildren<AbilityUIController>(true);
    }

    public void AbilityEquip(int i)
    {
        // takes integer from Abilityslot, takes in data/SO from cursel and links to AbilityHolder to equip
        if (cursel == null)
        {
            return;
        }
        AHolder[i].ability = cursel.data;

        //print("Buttonpressedyo");
    }




    //public void UpdateUIValues()
    //{
    //    PlayerInventory = _inter.interactor.GetComponent<PlayerInventory>();
    //    GetCurrencyValue();
    //    foreach (var item in StatUpgrades)
    //    {
    //        item.UpgradeStore = this;
    //        item.Initialize();
    //    }
    //}

    //public void GetCurrencyValue()
    //{
    //    ChaosCoresTxt.text = $"CC: {_inter.interactor.GetComponent<PlayerInventory>().ChaosCores}";
    //}









}
