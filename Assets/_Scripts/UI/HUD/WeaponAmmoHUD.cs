using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmoHUD : MonoBehaviour
{
    public AmmoPip AmmoPipPrefab;
    public Transform PipParent;
    List<AmmoPip> Pips = new List<AmmoPip>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    int currentAmmo;
    int MaxAmmo;


    // Update is called once per frame
    void Update()
    {
        if (Pips.Count == 0) return;
        currentAmmo = WeaponHolder.Singleton.GetWeaponAmmo();
        if (currentAmmo < MaxAmmo)
        {
            if (currentAmmo > 0)
            {
                Pips[currentAmmo].SetFill(WeaponHolder.Singleton.GetWeaponFractionalRegen());
            }
            else
            {
                Pips[0].SetFill(WeaponHolder.Singleton.GetWeaponFractionalRegen());
            }
            

        }

        for (int i = 0; i < currentAmmo; i++)
        {
            Pips[i].AmmoReady();
        }
        for(int i = currentAmmo;  i < Pips.Count; i++)
        {
            Pips[i].AmmoUsed();
            if (i == currentAmmo) continue;
            Pips[i].SetFill(0);
        }



    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void UseAmmo()
    {

    }


    public void SetAmmoPips(int MaxAmmoCount)
    {
        foreach(var pip in Pips) Destroy(pip.gameObject);
        Pips.Clear();
        MaxAmmo = MaxAmmoCount;
        for (int i = 0; i < MaxAmmoCount; i++)
        {
            var pip = Instantiate(AmmoPipPrefab, PipParent);
            Pips.Add(pip);
        }
    }
}
