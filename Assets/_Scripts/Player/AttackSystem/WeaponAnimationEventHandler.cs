using UnityEngine;
using UnityEngine.Events;

public class WeaponAnimationEventHandler : MonoBehaviour
{
    public UnityEvent onComboWindowOpen;
    public UnityEvent onComboWindowClosed;
    public UnityEvent onFinishedAttack;
    public UnityEvent OnAttackPerformed;
    public UnityEvent onMoveSpeedReset;
    
    CamAttackAnim CamAnimator;

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


    public void CloseComboWindow()
    {
        onComboWindowClosed.Invoke();
    }

    public void PlaySound(PlayerSoundSource.SoundType soundType)
    {
        PlayerSoundSource.instance.PlaySound(soundType, 1);
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
