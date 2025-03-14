using UnityEngine;

[CreateAssetMenu(fileName = "JumpAbility", menuName = "JumpAbility")]
public class JumpAbility : Ability
{
    public float jumpVelocity;

    public override void Activate(GameObject parent)
    {
        PlayerMovement movement = parent.GetComponent<PlayerMovement>();
        Rigidbody rigidbody = parent.GetComponent<Rigidbody>();

        movement.jumpEnhance = 5f;
    }
}
