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

    public IEnumerator Hurt()
    {
        if(currentHurtCorutine != null)
        {
            StopCoroutine(currentHurtCorutine);
        }
        
        fullScreenDamage.SetActive(true);
        material.SetFloat(_VoronoiIntensity, VORONOI_INTENSITY_START_AMOUNT);
        material.SetFloat(_VingnetteIntensity, VIGNETTE_INTENSITY_START_AMOUNT);
        yield return new WaitForSeconds(HitDisplayTime);

        float elapsedTime = 0f;
        while(elapsedTime < HitFadeoutTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedVoronoi = Mathf.Lerp(VORONOI_INTENSITY_START_AMOUNT, 0, (elapsedTime/ HitFadeoutTime));
            float lerpedVignette = Mathf.Lerp(VIGNETTE_INTENSITY_START_AMOUNT, 0, (elapsedTime / HitFadeoutTime));

            material.SetFloat(_VoronoiIntensity, lerpedVoronoi);
            material.SetFloat(_VingnetteIntensity, lerpedVignette);

            yield return null;
        }

        fullScreenDamage.SetActive(false);
        currentHurtCorutine = null; // clear reference
    }
}
