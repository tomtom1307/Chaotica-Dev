using UnityEngine;

public class Knockbackable : MonoBehaviour, IKnockbackable
{
    public virtual void GetKnockedBack(Vector3 force)
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(force);
        }
    }

    public virtual void GetKnockedBack(Vector3 force, Vector3 point)
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForceAtPosition(force, point);
        }
    }
}
