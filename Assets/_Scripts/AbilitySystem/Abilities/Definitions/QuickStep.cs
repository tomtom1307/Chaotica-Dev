using UnityEngine;

[CreateAssetMenu(fileName = "QuickStep", menuName = "Abilities/QuickStep")]
public class QuickStep : Ability
{
    public float StepSpeed;
    public override void Activate(GameObject parent, AbilityHolder holder)
    {
        base.Activate(parent, holder);
        PlayerMovement pm = parent.GetComponent<PlayerMovement>();
        pm.ResetYVelocity();
        Vector3 Dir;
        if (pm.OnSlope())
        {
            Dir = pm.slopeMoveDirection;
        }
        else
        {

            Dir = pm.MoveDir;
        }
        Rigidbody rb = parent.GetComponent<Rigidbody>();
       
        rb.AddForce(StepSpeed * Dir);
    }
}
