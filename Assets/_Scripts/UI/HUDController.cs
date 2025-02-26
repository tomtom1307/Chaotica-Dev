using System.Collections;
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

    #region ChargeMeter
    private Coroutine fillCoroutine;


    public void SetChargeMeterFill(float value)
    {
        ChargeMeter.fillAmount = value;
    }

    public void StartFill(float duration)
    {
        ChargeMeter.gameObject.SetActive(true);
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(FillOverTime(duration));
    }

    public void StopFill()
    {
        ChargeMeter.gameObject.SetActive(false);

        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }
    }

    private IEnumerator FillOverTime(float duration)
    {
        float elapsedTime = 0f;
        float startFill = ChargeMeter.fillAmount;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            ChargeMeter.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }

        ChargeMeter.fillAmount = 1f; // Ensure it reaches exactly 1
        fillCoroutine = null;
    }

    #endregion

}
