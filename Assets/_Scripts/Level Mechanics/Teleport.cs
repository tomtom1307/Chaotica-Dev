using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] Vector3 displacement;

    // Moves player to destination or alternatively displaces them by some vector.
    private void OnTriggerEnter(Collider other)
    {
        if(!enabled) return;
        if (destination != null)
        {
            other.transform.position = destination.position; other.transform.rotation = destination.rotation;
        }
        else if (displacement != null)
        {
            other.attachedRigidbody.MovePosition(displacement);
        }
    }
}
