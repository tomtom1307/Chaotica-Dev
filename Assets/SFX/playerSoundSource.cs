using System;
using UnityEngine;

public class PlayerSoundSource : SourceSoundManager<PlayerSoundSource.SoundType>
{
    public static PlayerSoundSource instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public override void PlaySound(SoundType sound, float volume, bool looping = false)
    {
        base.PlaySound(sound, volume, looping);
    }

    public override void StopSound()
    {
        base.StopSound();
    }

    public new enum SoundType
    {
        FootSteps,
        TakeDamage,
        SwingSword,
        DrawBow,
        ShootBow

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
