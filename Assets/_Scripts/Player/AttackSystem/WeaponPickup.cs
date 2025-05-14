using UnityEngine;
using UnityEngine.Events;

public class WeaponPickup : MonoBehaviour
{
    public WeaponInstance instance;
    public Interactable Interactable;
    public void Pickup()
    {
        Interactable = GetComponent<Interactable>();
        Interactable.interactor.GetComponent<WeaponHolder>().SetWeaponInstance(instance);
    }



}
