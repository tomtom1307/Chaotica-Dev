using System.Collections.Generic;
using UnityEngine;

public class EAI_AnimationEventHandler : MonoBehaviour
{
    public EAI_AnimatorController controller;
    public EnemyContext ctx;
    public bool ApplyRootMotion;


    private void OnAnimatorMove()
    {
        if (!ApplyRootMotion) return;
        Vector3 delta = ctx.anim.animator.deltaPosition;
        delta.y = 0;
        Vector3 newPos = transform.position + delta;
        ctx.motor.SetPosition(newPos);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TRTools.Helpers.TryFind<EAI_AnimatorController>(gameObject, out controller);
        TRTools.Helpers.TryFind<EnemyContext>(gameObject, out ctx);
    }


    public void ExecuteCurrentAttack()
    {
        ctx.attackHandler.ExecuteCurrentAttack();
    }

    public void DoColliderCheckAnim(int ColGroupIndex)
    {
        ctx.attackHandler.DoColliderCheck(ColGroupIndex);
    }

    public void DisableAllColliderCheck()
    {
        ctx.attackHandler.DisableAllColliderGroup();
    }

    public void DisableColliderCheck(int i)
    {
        ctx.attackHandler.DisableColliderGroup(i);
    }

    public void FinishAttack()
    {
        ctx.attackHandler.AttackExit();
    }


    public void PlaySound(EnemySoundSource.SoundType soundType)
    {
        ctx.sound.PlaySound(soundType);
    }

    public void SpawnVFX(int i)
    {
        ctx.attackHandler.SpawnVFX(i);
    }


}
