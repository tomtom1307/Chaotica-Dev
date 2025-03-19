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

    private AbilityUIController AbilityUIController;

    [HideInInspector] public Interactable _inter;
    [HideInInspector] public PlayerInventory PlayerInventory;

    public TMP_Text ChaosCoresTxt;

    //Function Called by interactable 
    public void _EnterAbilityWindow()
    {
        //UpdateUIValues();
        UIManager.instance.IsMenu(true, Canvas);
        UIManager.instance.ShowCursor();

        // Debug.Log(AbilityUIController);

        Player = _inter.interactor;

        
    }

    //List<StatUpgradeContainer> StatUpgrades;

    private void Start()
    {
       _inter = GetComponent<Interactable>();

       // AbilityUIController = GetComponentInChildren<AbilityUIController>(true);
    }

    public void OnJumpAbilityButtonClicked()
    {
        if (Player.GetComponent<AbilityHolder>().enabled == false)
        {
            // Activates Jump ability on Character
            print("Hey itss enabled");

            Player.GetComponent<AbilityHolder>().enabled = true;
        }
        else
        {
            // Disables the Jump ability on Character
            Debug.Log("Hey it is now disabled");

            Player.GetComponent<AbilityHolder>().enabled = false;
        }

        print("Buttonpressedyo");
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
