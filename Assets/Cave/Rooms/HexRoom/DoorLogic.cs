using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    public bool Opened;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleDoor(!Opened);
        }
    }

    public void ToggleDoor(bool x)
    {
        Opened = x;
        anim.SetBool("Opened", Opened);
    }
}
