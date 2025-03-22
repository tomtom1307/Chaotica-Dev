using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public int i;

    public AbilitySystemUI abilitySystemUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilitySystemUI = GetComponentInParent<AbilitySystemUI>();
    }

    public void OnButtonClicked()
    {
        abilitySystemUI.AbilityEquip(i);

        GetComponent<UnityEngine.UIElements.Image>().tintColor = Color.black;
    }
}
