using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AbilitySlot : MonoBehaviour
{
    public int i;

    public AbilitySystemUI abilitySystemUI;

    // public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilitySystemUI = GetComponentInParent<AbilitySystemUI>();
    }

    public void OnButtonClicked()
    {
        abilitySystemUI.AbilityEquip(i);

        if (abilitySystemUI.cursel == null)
        {
            return;
        }
        gameObject.GetComponent<UnityEngine.UI.Image>().sprite = abilitySystemUI.cursel.GetComponent<UnityEngine.UI.Image>().sprite;
    }
}
