using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FovChannel { Sprint, Crouch, Slide, Zoom, Ability, Impulse }

public class FOVFXController : MonoBehaviour
{
    [Header("Cameras")]
    public List<Camera> cams;

    [Header("Limits & Smoothing")]
    public float minFOV = 20f;
    public float maxFOV = 95f;
    public float smoothTime = 0.15f; // seconds to settle

    public static FOVFXController instance;

    // per-camera state
    private readonly Dictionary<Camera, float> baseFov = new();
    private readonly Dictionary<Camera, float> vel = new();

    // channel offsets (degrees)
    private readonly Dictionary<FovChannel, float> offsets = new();

    void Awake()
    {
        if (instance == null) instance = this; else { Destroy(this); return; }
        if (cams == null || cams.Count == 0)
        {
            Debug.LogWarning("FOVFXController: No cameras assigned!");
            enabled = false; return;
        }

        foreach (var c in cams)
        {
            baseFov[c] = c.fieldOfView; // keep each camera's own baseline
            vel[c] = 0f;
        }
    }

    // Set a channel’s offset (e.g., +5 for sprint, -20 for zoom)
    public void SetOffset(FovChannel ch, float value) => offsets[ch] = value;

    // Clear a channel (equivalent to 0)
    public void ClearOffset(FovChannel ch) { if (offsets.ContainsKey(ch)) offsets.Remove(ch); }

    void Update()
    {
        float sum = 0f;
        foreach (var kv in offsets) sum += kv.Value;

        foreach (var cam in cams)
        {
            float target = Mathf.Clamp(baseFov[cam] + sum, minFOV, maxFOV);
            float v = vel[cam];
            // unscaled delta so slow-mo doesn’t affect smoothing
            float f = Mathf.SmoothDamp(cam.fieldOfView, target, ref v, smoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
            cam.fieldOfView = f;
            vel[cam] = v;
        }
    }

    // Optional: additive “punch” (impulse) on top of everything else
    public void PlayImpulse(float amplitude, float inTime, float outTime) => StartCoroutine(Impulse(amplitude, inTime, outTime));
    private IEnumerator Impulse(float amp, float inT, float outT)
    {
        SetOffset(FovChannel.Impulse, 0f);

        float t = 0f;
        while (t < inT) { t += Time.unscaledDeltaTime; SetOffset(FovChannel.Impulse, Mathf.Lerp(0f, amp, t / inT)); yield return null; }
        t = 0f;
        while (t < outT) { t += Time.unscaledDeltaTime; SetOffset(FovChannel.Impulse, Mathf.Lerp(amp, 0f, t / outT)); yield return null; }

        ClearOffset(FovChannel.Impulse);
    }
}
