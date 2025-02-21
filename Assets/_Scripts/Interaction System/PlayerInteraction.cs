using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float PlayerReach = 3f;
    Interactable currentInteractable;

    private void Update()
    {
        CheckInteraction();


        if (Input.GetKeyDown(KeyCode.F) && currentInteractable )
        {
            currentInteractable.Interact();
        }
    }


    void CheckInteraction()
    {
        //Do Raycast to check
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        //If Ray Hit
        if (Physics.Raycast(ray, out hit, PlayerReach))
        {
            if(hit.collider.tag == "Interactable") //If looking at an interactable object
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                //If looking at a new interactable
                if(currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                }

                //If interatable is enabled then set new to current interactable
                if(newInteractable.enabled) { 
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable interactable){
        currentInteractable = interactable;
        currentInteractable.EnableOutline();
        HUDController.instance.EnableInteractionText(currentInteractable.message);
    }


    void DisableCurrentInteractable()
    {
        HUDController.instance.DisableInteractionText();
        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }



}
