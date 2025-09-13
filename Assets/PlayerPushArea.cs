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
        Rigidbody _rb = other.gameObject.GetComponent<Rigidbody>();
        if(_rb != null ) _rb.AddForce(Force * transform.forward);
    }
}
