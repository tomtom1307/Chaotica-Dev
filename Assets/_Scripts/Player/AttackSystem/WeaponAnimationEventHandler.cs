using UnityEngine;
using UnityEngine.Events;

public class WeaponAnimationEventHandler : MonoBehaviour
{
    public UnityEvent onComboWindowOpen;
    public UnityEvent onComboWindowClosed;
    public UnityEvent onFinishedAttack;
    public UnityEvent OnAttackPerformed;

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

    public void CloseComboWindow()
    {
        onComboWindowClosed.Invoke();
    }



}
