using UnityEngine;

public class Food : Interactable
{
    public float HealAmount;


    public override void Interact(GameObject player)
    {
        base.Interact(player);

        PlayerHealth.instance.Heal(HealAmount);
        //DO SFX AND VFX
        PlayerSoundSource.instance.PlaySound(PlayerSoundSource.SoundType.Eat);
        Destroy(gameObject);
    }


}
