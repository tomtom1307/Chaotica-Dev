using UnityEngine;
using UnityEngine.Events;

public class WeaponAnimationEventHandler : MonoBehaviour
{
    public UnityEvent onComboWindowOpen;
    public UnityEvent onComboWindowClosed;
    public UnityEvent onParryWindowOpen;
    public UnityEvent onParryWindowClosed;
    public UnityEvent onFinishedAttack;
    public UnityEvent OnAttackPerformed;
    public UnityEvent onMoveSpeedReset;
    public UnityEvent<int> OnVFXCalled;
    public UnityEvent<int> onForceApply;
    
    CamAttackAnim CamAnimator;

    public void OnForceApply(int i)
    {
        onForceApply.Invoke(i);
    }

    public void FinishedAttack()
    {
        onFinishedAttack.Invoke();
    }

    public void PerformAttack()
    {
        OnAttackPerformed.Invoke();
    }
    public void OpenComboWindow()
    {
        onComboWindowOpen.Invoke();
    }

    public void ResetMoveSpeed()
    {
        onMoveSpeedReset.Invoke();
    }

    public void OnVfxSet(int i)
    {
        OnVFXCalled.Invoke(i);
    }


    public void CloseComboWindow()
    {
        onComboWindowClosed.Invoke();
    }

    public void PlaySound(PlayerSoundSource.SoundType soundType)
    {
        PlayerSoundSource.instance.PlaySound(soundType);
    }

    //Can Later have it calculate the projection of the direction vector to the hand onto the camera plane instead of using enum
    public void CamSwing(SwingDirection dir)
    {
        CamAnimator.DoAnim(dir);
    }
    private void Start()
    {
        CamAnimator = GetComponentInParent<CamAttackAnim>();
    }

    public void OpenParryWindow()
    {
        onParryWindowOpen.Invoke();
    }

    public void CloseParryWindow()
    {
        onParryWindowClosed.Invoke();
    }


}

public enum SwingDirection
{
    Up,
    Down,
    Left,
    Right,
    //45 degree linear combos of the basis
    UL,
    UR,
    DL,
    DR
}
