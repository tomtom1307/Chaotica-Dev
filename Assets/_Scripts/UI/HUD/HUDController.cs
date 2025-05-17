using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;
    public GameObject DetectionMeter;
    public Transform DetectionTransform;
    public CanvasGroup DetectionUICanvasGroup;
    [SerializeField] Image ChargeMeter;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


    public List<DetectionIndicator> DetectionIndicators;

    [SerializeField] TMP_Text InteractionText;
    [SerializeField] TMP_Text ChaosCoreAmount;

    public DetectionIndicator TriggerDetectionMeter(EnemyBrain EB)
    {
        GameObject go = Instantiate(DetectionMeter, DetectionTransform);
        DetectionIndicator DI = go.GetComponent<DetectionIndicator>();
        DetectionIndicators.Add(DI);
        DI.enemy = EB;
        return DI;
    }


    public void SetChaosCoreText(float amount)
    {
        ChaosCoreAmount.text = amount.ToString();
    }

    public void EnableInteractionText(string text = "")
    {
        InteractionText.text = text + "[F]";
        InteractionText.gameObject.SetActive(true);
    }

    public void DisableInteractionText() {
        InteractionText.gameObject.SetActive(false);
    }

    private void Update()
    {
        foreach (var indicator in DetectionIndicators) {
            if (indicator.Agro)
            {
                StartCoroutine(FadeOut(DetectionUICanvasGroup, 1.5f));
                return;
            }
        }
        DetectionUICanvasGroup.alpha = 1;
    }

    public IEnumerator FadeOut(CanvasGroup CG,float FadeTime)
    {
        float timer = FadeTime;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            CG.alpha = Mathf.Clamp01(timer / FadeTime);

            yield return null; // Wait one frame
        }

        CG.alpha = 0f; // Make sure it ends at 0
        
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
