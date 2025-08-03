using System;
using UnityEngine;

public class EnemySoundSource : SourceSoundManager<EnemySoundSource.SoundType>
{
    public override void PlaySound(SoundType sound, float volume, bool looping = false, float pitch = -1, float DetectionRadius = -1)
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
        TakeDamageBladed,
        TakeDamageBlunt,
        AttackSFX,
        AttackChargeup
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
