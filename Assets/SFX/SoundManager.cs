using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public abstract class SourceSoundManager<T> : MonoBehaviour where T: Enum
{
    public enum SoundType
    {
        
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    [HideInInspector]public AudioSource audioSource;
    public SoundList[] clips;
    public virtual void PlaySound(T sound, float volume = -1, bool looping = false, float pitch = -1)
    {
        
        AudioClip[] _clips = clips[Convert.ToInt32(sound)].Sounds;
        if(_clips.Length == 0) { Debug.LogError("The Called sound has no assigned sounds please assign them in the inspector"); return; }
        
        AudioClip randomClip = _clips[UnityEngine.Random.Range(0, _clips.Length)];
        if(volume == -1)
        {
            volume = clips[Convert.ToInt32(sound)].desiredVolume;
        }
        float finalPitch = 1;
        if(pitch == -1)
        {
            finalPitch = UnityEngine.Random.Range(clips[Convert.ToInt32(sound)].pitchVariation.x, clips[Convert.ToInt32(sound)].pitchVariation.y);
        }
        else
        {
            audioSource.pitch = finalPitch;
        }
        if (looping)
        {
            audioSource.loop = true;
            audioSource.clip = randomClip;
            audioSource.volume = volume;
            audioSource.Play();
        }
        else
        {
            audioSource.loop = false;
            audioSource.clip = null;
            audioSource.PlayOneShot(randomClip, volume);
        }


    }

    public virtual void StopSound()
    {
        audioSource.clip = null;
        audioSource.Stop();
    }


#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref clips, names.Length);
        for (int i = 0; i < clips.Length; i++)
        {
            clips[i].name = names[i];
        }
    }
#endif

}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] public Vector2 pitchVariation;
    [SerializeField] public float desiredVolume;
    [SerializeField] public AudioClip[] sounds;
}



