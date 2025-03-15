using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStash : MonoBehaviour
{
    public GameObject canvas;

    public Transform WeaponsContainer;
    public GameObject WeaponItemPrefab;
    public Image ProgressBar;

    public Interactable interactable;
    private void Start()
    {
        interactable = GetComponent<Interactable>();
        weaponStashObjs = new List<GameObject>();
    }


    public void OpenWeaponStashMenu()
    {
        UIManager.instance.IsMenu(true, canvas);
        UIManager.instance.ShowCursor();
        DisplayUnlockedWeapons();
    }

    WeaponHolder weaponHolder;


    List<GameObject> weaponStashObjs;
    public void DisplayUnlockedWeapons()
    {
        if(weaponStashObjs.Count > 0)
        {
            foreach (var item in weaponStashObjs)
            {
                Destroy(item);
            }
        }
        
        weaponStashObjs = new List<GameObject>();
        ProgressBar.fillAmount = 0;
        PlayerInventory inv = interactable.interactor.GetComponent<PlayerInventory>();
        weaponHolder = inv.gameObject.GetComponent<WeaponHolder>();

        foreach (var weaponInstance in inv.weaponStashDatas)
        {
            GameObject inst = Instantiate(WeaponItemPrefab, WeaponsContainer);
            weaponStashObjs.Add(inst);
            inst.GetComponent<WeaponStashItem>().SetData(weaponInstance, this);
            if (weaponHolder.data == weaponInstance.data)
            {
                currentSelectedWeapon = inst.GetComponent<WeaponStashItem>();
                Equip();
                SelectCurrentWeapon(currentEquipedWeapon);
            }
            
        }


    }

    WeaponStashItem currentSelectedWeapon;
    [SerializeField] WeaponStashItem currentEquipedWeapon;

    public void SelectCurrentWeapon(WeaponStashItem currentWeapon)
    {
        if (currentSelectedWeapon != null)
        {
            currentSelectedWeapon.SetSelected(false);
        }
        currentSelectedWeapon = currentWeapon;
        currentSelectedWeapon.SetSelected(true);
        PopulateInfo();
    }
    public TMP_Text NameTxt;
    public TMP_Text DamageText;
    
    public void PopulateInfo()
    {
        NameTxt.text = currentSelectedWeapon.data._Name;
        DamageText.text = "Damage: " + currentSelectedWeapon.data.WeaponDamage;
        ProgressBar.fillAmount = currentSelectedWeapon.instance.KillCount / (currentSelectedWeapon.instance.Threshold1 + currentSelectedWeapon.instance.Threshold2);
        HandleAttackInfoContainers(currentSelectedWeapon.instance);
    }

    public void Equip()
    {
        Debug.Log("Equip!");
        if(currentEquipedWeapon != null)
        {
            currentEquipedWeapon.SetEquiped(false);
        }
        currentEquipedWeapon = currentSelectedWeapon;
        weaponHolder.SetWeaponInstance(currentEquipedWeapon.instance);


        currentEquipedWeapon.SetEquiped(true);
    }


    [SerializeField] List<WeaponAttackItem> AttackContainers;

    public void HandleAttackInfoContainers(WeaponInstance inst)
    {
        foreach (var container in AttackContainers)
        {
            container.FillInfo(inst);
        }
    }



}
