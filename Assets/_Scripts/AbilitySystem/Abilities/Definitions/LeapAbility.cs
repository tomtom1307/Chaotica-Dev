using UnityEngine;

[CreateAssetMenu(fileName = "leapAbility", menuName = "Abilities/leapAbility")]
public class LeapAbility : Ability
{
    public float LeapForce;
    public float UpForce;

    public override void Activate(GameObject parent)
    {
        //PlayerMovement movement = parent.GetComponent<PlayerMovement>();
        Rigidbody rigidbody = parent.GetComponent<Rigidbody>();

        rigidbody.AddForce(Camera.main.transform.forward*LeapForce+Vector3.up*UpForce);
    }
}
