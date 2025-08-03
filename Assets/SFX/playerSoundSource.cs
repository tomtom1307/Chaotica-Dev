using System;
using System.Collections.Generic;
using System.Linq;
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

    public override void PlaySound(SoundType sound, float volume = -1, bool looping = false, float pitch = -1, float DetectionRadius = -1)
    {
        
        base.PlaySound(sound, volume, looping, pitch);

        if (DetectionRadius == -1)
        {
            DoDetectionSphere(clips[Convert.ToInt32(sound)].DetectionRadius);
        }
        else
        {
            DoDetectionSphere(DetectionRadius);
        }
           
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
        ShootBow,
        CrystalPickup,
        Parry,
        Sliding,
        SlideStop
    }



    public void DoDetectionSphere(float Range)
    {
        List<Collider> cols = Physics.OverlapSphere(transform.position, Range).ToList();
        foreach (Collider col in cols)
        {
            EnemyBrain EB;
            if (col.gameObject.TryGetComponent<EnemyBrain>(out EB))
            {
                EB.actionHandler.StartActionOverride(EB.actionHandler.MoveToPlayer);
            }
        }

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
