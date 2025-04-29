using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public float ChaosCores;

    public List<WeaponInstance> weaponStashDatas;

    private void Start()
    {
        CrystalPickupPitch = 1;
        HUDController.instance.SetChaosCoreText(ChaosCores);
    }
    public float CrystalPickupPitch;
    public float CrystalPickupPitchDecay;
    public float CrystalPickupPitchBump;
    private void Update()
    {
        CrystalPickupPitch = Mathf.Clamp(CrystalPickupPitch - CrystalPickupPitchDecay*Time.deltaTime, 1, 10);
    }

    public void AddChaosCores(float Amount)
    {
        ChaosCores += Amount;
        CrystalPickupPitch += CrystalPickupPitchBump;
        PlayerSoundSource.instance.PlaySound(PlayerSoundSource.SoundType.CrystalPickup, -1, false, CrystalPickupPitch);
        HUDController.instance.SetChaosCoreText(ChaosCores);
    }

}
