using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Throwable : Interactable
{
    public float ThrowForce;
    public Vector3 Orientation;
    Rigidbody rb;
    public override void Start()
    {
      rb = GetComponent<Rigidbody>(); 
      base.Start();
    }


    public override void Interact(GameObject player)
    {
        base.Interact(player);
        
        Transform CamTransform = Camera.main.transform;
        if(Orientation != Vector3.zero )
        {
            transform.rotation = Quaternion.LookRotation(CamTransform.right * Orientation.x + CamTransform.up * Orientation.y + CamTransform.forward * Orientation.z, Vector3.up);
        }
        Vector3 dir = Vector3.ProjectOnPlane(CamTransform.forward, Vector3.up);
        rb.AddForce(ThrowForce * dir);


    }
}
