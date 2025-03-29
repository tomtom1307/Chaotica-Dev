using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AbilitySlot : MonoBehaviour
{
    public int SlotNumber;

    public AbilitySystemUI abilitySystemUI;

    public GameObject AEquip;

    // public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilitySystemUI = GetComponentInParent<AbilitySystemUI>();

    }

    public void OnButtonClicked()
    {
        abilitySystemUI.AbilityEquip(SlotNumber);

        if (abilitySystemUI.cursel == null)
        {
            return;
        }
        gameObject.GetComponent<UnityEngine.UI.Image>().sprite = abilitySystemUI.cursel.GetComponent<UnityEngine.UI.Image>().sprite;
    }

    public void OnEnableHover()
    {
        if (abilitySystemUI.cursel == null 
            || abilitySystemUI.cursel.GetComponent<UnityEngine.UI.Image>().sprite == gameObject.GetComponent<UnityEngine.UI.Image>().sprite)
        {
            return;
        }

        AEquip.SetActive(true);
    }

    public void OnDisableHover()
    {
        AEquip.SetActive(false);
    }
}
