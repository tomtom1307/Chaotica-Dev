using UnityEngine;
using UnityEngine.UI;

public class WeaponStashItem : MonoBehaviour
{
    public WeaponDataSO data;
    public Image SpriteDisplay;
    WeaponStash ws;
    public GameObject SelectedBorder;
    public GameObject EquipedBorder;

    public bool Selected;
    public bool Equiped;

    public void SetData(WeaponDataSO data, WeaponStash WS)
    {
        this.data = data;
        SetSelected(false);
        SetEquiped(false);
        SpriteDisplay.sprite = this.data.InventorySprite;
        ws = WS;
    }


    public void Select()
    {
        ws.SelectCurrentWeapon(this);
    }

    public void SetSelected(bool x)
    {

        SelectedBorder.SetActive(x);
        Selected = x;
    }

    public void SetEquiped(bool x)
    {
        EquipedBorder.SetActive(x);
        Equiped = x;
    }


}
