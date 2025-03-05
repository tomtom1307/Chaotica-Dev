using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponStash : MonoBehaviour
{
    public GameObject canvas;

    public Transform WeaponsContainer;
    public GameObject WeaponItemPrefab;

    public Interactable interactable;
    private void Start()
    {
        interactable = GetComponent<Interactable>();
        
    }


    public void OpenWeaponStashMenu()
    {
        UIManager.instance.IsMenu(true, canvas);
        UIManager.instance.ShowCursor();
        DisplayUnlockedWeapons();
    }

    public List<WeaponDataSO> datas;
    WeaponHolder weaponHolder;

    public void DisplayUnlockedWeapons()
    {

        PlayerInventory inv = interactable.interactor.GetComponent<PlayerInventory>();
        weaponHolder = inv.gameObject.GetComponent<WeaponHolder>();
        List<WeaponDataSO> list = inv.weaponStashDatas;

        foreach (var weaponData in list)
        {
            
            if (datas.Contains(weaponData))
            {
                continue;
            }
            
            GameObject inst = Instantiate(WeaponItemPrefab, WeaponsContainer);
            datas.Add(weaponData);
            inst.GetComponent<WeaponStashItem>().SetData(weaponData, this);
            if (weaponHolder.data == weaponData)
            {
                currentSelectedWeapon = inst.GetComponent<WeaponStashItem>();
                Equip();
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
    }

    public void Equip()
    {
        Debug.Log("Equip!");
        if(currentEquipedWeapon != null)
        {
            currentEquipedWeapon.SetEquiped(false);
        }
        currentEquipedWeapon = currentSelectedWeapon;
        weaponHolder.SetWeaponData(currentEquipedWeapon.data);


        currentEquipedWeapon.SetEquiped(true);
    }


}
