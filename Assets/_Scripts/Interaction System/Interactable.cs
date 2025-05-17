using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    Outline outline;
    public string message;
    public bool DoOutline;

    [HideInInspector] public GameObject interactor;
    public UnityEvent onInteraction = new UnityEvent();


    //Get Outline Component
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }


    public void Interact(GameObject player)
    {
        interactor = player;
        onInteraction.Invoke();

    }


    

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }


}
