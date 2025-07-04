using UnityEngine;
using UnityEngine.UI;

public class HUDAbility : MonoBehaviour
{
    public Color ActiveColor;
    public Color ReadyColor;

    public Image display;
    public Image CoolDownImage;
    public AbilityHolder AH;

    public void SetAbility(AbilityHolder abilityHolder)
    {
        AH = abilityHolder;
        AH.SetHUD(this);
        display.sprite = abilityHolder.ability.Display;
        CoolDownImage.sprite = abilityHolder.ability.Display;
    }

    public void AbilityReady()
    {
        CoolDownImage.fillAmount = 0;
    }

    public void AbilityActive()
    {
        display.color = ActiveColor;
    }

    public void AbilityCooldown(float CooldownTimer)
    {
        display.color = ReadyColor;
        CoolDownImage.fillAmount = CooldownTimer/AH.ability.cooldownTime;
    }


}
