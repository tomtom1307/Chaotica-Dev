using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerPushArea : MonoBehaviour
{
    public float Force;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        other.gameObject.GetComponent<Rigidbody>().AddForce(Force*transform.forward);
    }
}
