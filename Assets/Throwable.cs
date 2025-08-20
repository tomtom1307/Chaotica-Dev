using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Throwable : Interactable
{
    public float ThrowForce;
    Rigidbody rb;
    private void Start()
    {
      rb = GetComponent<Rigidbody>();  
    }


    public override void Interact(GameObject player)
    {
        base.Interact(player);
        Transform CamTransform = Camera.main.transform;
        rb.AddForce(ThrowForce * CamTransform.forward);


    }
}
