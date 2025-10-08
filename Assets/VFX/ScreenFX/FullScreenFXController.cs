using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FullScreenFXController : MonoBehaviour
{
    public float HitDisplayTime;
    public float HitFadeoutTime;

    public ScriptableRendererFeature fullScreenDamage;
    public Material material;

    private int _VoronoiIntensity = Shader.PropertyToID("_VoronoiIntensity");
    private int _VingnetteIntensity = Shader.PropertyToID("_VingnetteIntensity");

    public float VORONOI_INTENSITY_START_AMOUNT = 1.8f;
    public float VIGNETTE_INTENSITY_START_AMOUNT = 0.5f;
    public Coroutine currentHurtCorutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fullScreenDamage.SetActive(false);
    }

    private void Update()
    {
        
    }
    private Coroutine currentHurtCoroutine;

    public void TriggerHurt()
    {
        // If already running, restart it
        if (currentHurtCoroutine != null)
            StopCoroutine(currentHurtCoroutine);

        currentHurtCoroutine = StartCoroutine(Hurt());
    }

    private IEnumerator Hurt()
    {
        fullScreenDamage.SetActive(true);
        material.SetFloat(_VoronoiIntensity, VORONOI_INTENSITY_START_AMOUNT);
        material.SetFloat(_VingnetteIntensity, VIGNETTE_INTENSITY_START_AMOUNT);

        yield return new WaitForSeconds(HitDisplayTime);

        float elapsedTime = 0f;
        while (elapsedTime < HitFadeoutTime)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / HitFadeoutTime;
            material.SetFloat(_VoronoiIntensity, Mathf.Lerp(VORONOI_INTENSITY_START_AMOUNT, 0, t));
            material.SetFloat(_VingnetteIntensity, Mathf.Lerp(VIGNETTE_INTENSITY_START_AMOUNT, 0, t));

            yield return null;
        }

        material.SetFloat(_VoronoiIntensity, 0);
        material.SetFloat(_VingnetteIntensity, 0);
        fullScreenDamage.SetActive(false);
        currentHurtCoroutine = null;
    }

}
