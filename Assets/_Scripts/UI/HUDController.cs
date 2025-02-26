using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;
    [SerializeField] Image ChargeMeter;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] TMP_Text InteractionText;

    public void EnableInteractionText(string text = "")
    {
        InteractionText.text = text + "[F]";
        InteractionText.gameObject.SetActive(true);
    }

    public void DisableInteractionText() {
        InteractionText.gameObject.SetActive(false);
    }


    public void SetChargeMeterFill(float value)
    {
        ChargeMeter.fillAmount = value;
    }


}
