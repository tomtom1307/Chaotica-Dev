using UnityEngine;

public class Knockbackable : MonoBehaviour, IKnockbackable
{
    public float Mult;

    public virtual void GetKnockedBack(Vector3 force)
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(Mult * force);
        }
    }

    public virtual void GetKnockedBack(Vector3 force, Vector3 point)
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForceAtPosition(Mult * force, point);
        }
    }
}
