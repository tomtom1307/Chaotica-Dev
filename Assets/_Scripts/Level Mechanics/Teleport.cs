using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] Vector3 displacement;

    // Moves player to destination or alternatively displaces them by some vector.
    private void OnTriggerEnter(Collider other)
    {
        if(!enabled) return;
        other.attachedRigidbody.linearVelocity = Vector3.zero;
        if (destination != null)
        {
            CameraController camController = Camera.main.transform.parent.gameObject.GetComponent<CameraController>();
            other.transform.position = destination.position;
            camController.OrientPlayer(destination.rotation);
        }
        else if (displacement != null)
        {
            other.attachedRigidbody.MovePosition(displacement);
        }
    }
}
