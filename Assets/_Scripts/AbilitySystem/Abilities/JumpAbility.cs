using UnityEngine;

[CreateAssetMenu(fileName = "JumpAbility", menuName = "JumpAbility")]
public class JumpAbility : Ability
{
    public float jumpForce;

    public override void Activate(GameObject parent)
    {
        //PlayerMovement movement = parent.GetComponent<PlayerMovement>();
        Rigidbody rigidbody = parent.GetComponent<Rigidbody>();

        rigidbody.AddForce(Vector3.up * jumpForce);
    }
}
