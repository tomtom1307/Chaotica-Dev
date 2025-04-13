using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public float ChaosCores;

    public List<WeaponInstance> weaponStashDatas;

    private void Start()
    {
        HUDController.instance.SetChaosCoreText(ChaosCores);
    }

    public void AddChaosCores(float Amount)
    {
        ChaosCores += Amount;
        HUDController.instance.SetChaosCoreText(ChaosCores);
    }

}
