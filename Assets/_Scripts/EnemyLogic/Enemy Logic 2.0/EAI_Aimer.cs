using UnityEngine;

public class Aimer : MonoBehaviour
{
    public bool flat_rotation;
    public float turnSpeed = 8f;
    public void AimAt(Vector3 worldPoint)
    {
        Vector3 flat = worldPoint - transform.position;
        if (flat_rotation) flat.y = 0f;
        if (flat.sqrMagnitude < 0.0001f) return;
        var q = Quaternion.LookRotation(flat.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, turnSpeed * Time.deltaTime);
    }
}

