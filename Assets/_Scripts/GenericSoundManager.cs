using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(AudioSource))]

public class GenericSoundManager : MonoBehaviour
{
    [HideInInspector]public AudioSource audioSource;
    public List<SoundSource> Sounds;
    public void PlaySound(int i)
    {
        if(audioSource == null) audioSource = GetComponent<AudioSource>();
        if (Sounds[i].clips.Count == 0) return;
        int RandomSelection = UnityEngine.Random.Range(0, Sounds[i].clips.Count - 1);
        audioSource.volume = Sounds[i].DefaultVolume;
        audioSource.PlayOneShot(Sounds[i].clips[RandomSelection]);
    }


    public void PlaySoundAtVol(int i, float Vol)
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (Sounds[i].clips.Count == 0) return;
        int RandomSelection = UnityEngine.Random.Range(0, Sounds[i].clips.Count - 1);
        audioSource.volume = Vol;
        audioSource.PlayOneShot(Sounds[i].clips[RandomSelection]);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public struct SoundSource
{
    public string name;
    public float DefaultVolume;
    public List<AudioClip> clips;
}
