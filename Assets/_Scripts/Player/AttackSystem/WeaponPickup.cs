using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPickup : Interactable
{
    public WeaponInstance instance;




    public override void Interact(GameObject player)
    {
        base.Interact(player);
        Pickup();
    }

    public void Pickup()
    {
        WeaponHolder WH = interactor.GetComponent<WeaponHolder>();
        if (WH.State == WeaponHolder.AttackState.Ready)
        {
            WH.SetWeaponInstance(instance);
            UpDown.Kill();
            rot.Kill();
            OnGazeStop();
            Destroy(this.gameObject);
            if (!WH.enabled) WH.enabled = true;
        }
        
        
    }



    // === Gaze hooks ===
    protected override void OnGazeHold()
    {
        // Player has looked for >= gazeThreshold seconds
        WeaponStatCanvas.instance.DisplayWeaponStats();
        WeaponStatCanvas.instance.PopulateData(instance);
    }

    protected override void OnGazeStop()
    {
        WeaponStatCanvas.instance.HideWeaponStats();
    }



    Tween UpDown;
    Tween rot;
    public override void Start()
    {
        base.Start();
        rot = transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 360, 0), 5,RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        UpDown = transform.DOMove(transform.position + 0.2f * Vector3.up, 5).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }


}
