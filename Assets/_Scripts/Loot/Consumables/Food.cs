using UnityEngine;
using UnityEngine.VFX;

public class Food : Interactable
{
    public float HealAmount;
    public Rarity rarity;
    GameObject RarityVFX;

    public override void Start()
    {
        base.Start();
        RarityVFX = GameManager.instance.SpawnRarityVFX(transform, rarity);
    }


    public override void Interact(GameObject player)
    {
        base.Interact(player);

        PlayerHealth.instance.Heal(HealAmount);
        //DO SFX AND VFX
        PlayerSoundSource.instance.PlaySound(PlayerSoundSource.SoundType.Eat);
        Destroy(gameObject);
    }




}
