using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ClimbRock : Interactable
{
    public Transform target;
    public float VertClimbForce;
    public float DirectClimbForce;

    public UnityEvent OnStoppedClimbing;


    public override void Interact(GameObject player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();


        //Reset y vel
        //rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.linearVelocity = Vector3.zero;

        //Calc direction
        Vector3 dir = TRTools.VecOp.DirectionDistance(player.transform.position, target.position);

        //Do force shit
        Vector3 Force = - DirectClimbForce * dir + VertClimbForce*Vector3.up;
        rb.AddForce(Force, ForceMode.VelocityChange);
        StartCoroutine(Climbed());
        base.Interact(player);
    }

    public IEnumerator Climbed()
    {
        yield return new WaitForSeconds(1);
        OnStoppedClimbing.Invoke();
    }

}
