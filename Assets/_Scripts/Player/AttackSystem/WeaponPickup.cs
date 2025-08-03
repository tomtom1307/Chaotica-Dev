using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPickup : MonoBehaviour
{
    public WeaponInstance instance;
    public Interactable Interactable;
    public void Pickup()
    {
        Interactable = GetComponent<Interactable>();
        WeaponHolder WH = Interactable.interactor.GetComponent<WeaponHolder>();
        if (WH.State == WeaponHolder.AttackState.Ready)
        {
            WH.SetWeaponInstance(instance);
            UpDown.Kill();
            rot.Kill();
            Destroy(this.gameObject);
            if (!WH.enabled) WH.enabled = true;
        }
        
        
    }
    Tween UpDown;
    Tween rot;
    private void Start()
    {
        
        rot = transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 360, 0), 5,RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        UpDown = transform.DOMove(transform.position + 0.2f * Vector3.up, 5).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }


}
