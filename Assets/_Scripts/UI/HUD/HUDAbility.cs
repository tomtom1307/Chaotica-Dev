using UnityEngine;
using UnityEngine.UI;

public class HUDAbility : MonoBehaviour
{
    public Image display;

    public void SetAbility(Ability ability)
    {
        display.sprite = ability.Display;
    }

}
