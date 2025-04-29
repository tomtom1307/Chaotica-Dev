using UnityEngine;

[CreateAssetMenu(fileName = "QuickStep", menuName = "Abilities/QuickStep")]
public class QuickStep : Ability
{
    public float StepSpeed;
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);
        PlayerMovement pm = parent.GetComponent<PlayerMovement>();
        Vector3 Dir;
        if (pm.OnSlope())
        {
            Dir = pm.slopeMoveDirection;
        }
        else
        {

            Dir = pm.MoveDir;
        }
        parent.GetComponent<Rigidbody>().AddForce(StepSpeed * Dir);
    }
}
