
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created]

    public static GameManager instance;

    public List<EnemyPatrolPoint> patrolPoints;
    public List<EnemyPatrolPoint> availablePatrolPoints;


    public Transform player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        availablePatrolPoints = new List<EnemyPatrolPoint>();
        player = GameObject.FindWithTag("Player").transform;
    }

    
    

    public EnemyPatrolPoint FindClosestPatrol(Vector3 pos)
    {
        float minDist = 1000;
        EnemyPatrolPoint closestPP = null;
        foreach (var item in availablePatrolPoints)
        {
            float Dist = Vector3.Distance(item.transform.position, pos);
            if (minDist > Dist)
            {
                closestPP = item;
                minDist = Dist;
            }
        }
        if(closestPP == null)
        {
            Debug.LogError("NO AVAILABLE CLOSEST PATROL POINT FOUND");
        }
        return closestPP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SlowTimeScale(0.5f, 2);
        }
    }


    private Vignette vignette;
    private ColorAdjustments colorAdjustments;
    private ChromaticAberration chromaticAberation;
    private Bloom bloom;

    public Volume postProcessVolume;

    [Range(0f, 1f)] public float vignetteTarget = 0.5f;
    [Range(-100f, 100f)] public float saturationTarget = -50f;
    [Range(0, 1)] public float chromaticAberationTarget = 0.5f;
    [Range(0, 5)] public float bloomTarget = 0.9f;
    float bloomInit;


    private void Start()
    {
        postProcessVolume.profile.TryGet(out vignette);
        postProcessVolume.profile.TryGet(out colorAdjustments);
        postProcessVolume.profile.TryGet(out chromaticAberation);
        postProcessVolume.profile.TryGet(out bloom);
        bloomInit = bloom.intensity.value;
    }

    public void SlowTimeScale(float slowAmount, float holdDuration, float lerpDuration = 0.2f)
    {
        StartCoroutine(SlowTimeCoroutine(slowAmount, holdDuration, lerpDuration));
    }

    private IEnumerator SlowTimeCoroutine(float slowAmount, float holdDuration, float lerpDuration)
    {
        float elapsed = 0f;
        float fxLerpDuration = 0.2f; // how fast FX fade in
        float baseFixedDelta = 0.02f;

        // === Step 1: FX + Time Lerp IN ===
        while (elapsed < fxLerpDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / fxLerpDuration;

            Time.timeScale = Mathf.Lerp(1f, slowAmount, t);
            Time.fixedDeltaTime = Mathf.Max(0.005f, baseFixedDelta * Time.timeScale);

            vignette.intensity.value = Mathf.Lerp(0f, vignetteTarget, t);
            colorAdjustments.saturation.value = Mathf.Lerp(0f, saturationTarget, t);
            chromaticAberation.intensity.value = Mathf.Lerp(0, chromaticAberationTarget, t);
            bloom.intensity.value = Mathf.Lerp(bloomInit, bloomTarget, t);
            yield return null;
        }

        // Lock at slow values
        Time.timeScale = slowAmount;
        Time.fixedDeltaTime = Mathf.Max(0.005f, baseFixedDelta * Time.timeScale);
        vignette.intensity.value = vignetteTarget;
        colorAdjustments.saturation.value = saturationTarget;

        // === Step 2: Hold Duration ===
        yield return new WaitForSecondsRealtime(holdDuration);

        // === Step 3: Lerp OUT (FX + Time) ===
        elapsed = 0f;
        while (elapsed < lerpDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / lerpDuration;

            Time.timeScale = Mathf.Lerp(slowAmount, 1f, t);
            Time.fixedDeltaTime = Mathf.Max(0.005f, baseFixedDelta * Time.timeScale);

            vignette.intensity.value = Mathf.Lerp(vignetteTarget, 0f, t);
            colorAdjustments.saturation.value = Mathf.Lerp(saturationTarget, 0f, t);
            chromaticAberation.intensity.value = Mathf.Lerp(chromaticAberationTarget, 0, t);
            bloom.intensity.value = Mathf.Lerp(bloomTarget, bloomInit, t);

            yield return null;
        }

        // Final cleanup
        Time.timeScale = 1f;
        Time.fixedDeltaTime = baseFixedDelta;
        vignette.intensity.value = 0f;
        colorAdjustments.saturation.value = 0f;
    }



    public void EnemyKilled()
    {
        if (player != null)
        {
            player.GetComponent<WeaponHolder>().EnemyKilled();
        }
    }
}
