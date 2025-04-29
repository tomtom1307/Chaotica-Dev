using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVFXController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float Intensity;

    public List<Camera> cams;
    public float DefaultFOV;
    public float DefaultDuration;
    private Coroutine currentFOVCoroutine;
    public static FOVFXController instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        if (cams == null || cams.Count == 0)
        {
            Debug.LogWarning("FOVFXController: No cameras assigned!");
            return;
        }
        DefaultFOV = cams[0].fieldOfView;
    }

    public void SetTargetFOV(float newFOV, float duration = -1)
    {
        if(duration == -1)
        {
            duration = DefaultDuration;
        }
        if (currentFOVCoroutine != null)
        {
            StopCoroutine(currentFOVCoroutine);
        }
        currentFOVCoroutine = StartCoroutine(LerpFOV(newFOV, duration));
    }

    public void PunchFOV(float punchFOV, float punchInDuration, float holdDuration, float punchOutDuration)
    {
        if (currentFOVCoroutine != null)
        {
            StopCoroutine(currentFOVCoroutine);
        }
        currentFOVCoroutine = StartCoroutine(PunchFOVRoutine(punchFOV, punchInDuration, holdDuration, punchOutDuration));
    }

    private IEnumerator LerpFOV(float targetFOV, float duration)
    {
        List<float> startFOVs = new List<float>();
        foreach (var cam in cams)
        {
            startFOVs.Add(cam.fieldOfView);
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            for (int i = 0; i < cams.Count; i++)
            {
                cams[i].fieldOfView = Mathf.Lerp(startFOVs[i], targetFOV, t);
            }

            yield return null;
        }

        foreach (var cam in cams)
        {
            cam.fieldOfView = targetFOV;
        }
    }

    private IEnumerator PunchFOVRoutine(float punchFOV, float punchInDuration, float holdDuration, float punchOutDuration)
    {
        // Store original FOVs
        List<float> originalFOVs = new List<float>();
        foreach (var cam in cams)
        {
            originalFOVs.Add(cam.fieldOfView);
        }

        // Punch in
        float elapsedTime = 0f;
        while (elapsedTime < punchInDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / punchInDuration);

            for (int i = 0; i < cams.Count; i++)
            {
                cams[i].fieldOfView = Mathf.Lerp(originalFOVs[i], punchFOV, t);
            }

            yield return null;
        }

        foreach (var cam in cams)
        {
            cam.fieldOfView = punchFOV;
        }

        // Hold
        yield return new WaitForSeconds(holdDuration);

        // Punch out
        elapsedTime = 0f;
        while (elapsedTime < punchOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / punchOutDuration);

            for (int i = 0; i < cams.Count; i++)
            {
                cams[i].fieldOfView = Mathf.Lerp(punchFOV, originalFOVs[i], t);
            }

            yield return null;
        }

        for (int i = 0; i < cams.Count; i++)
        {
            cams[i].fieldOfView = originalFOVs[i];
        }
    }
}
