using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PhysicsPushArea : MonoBehaviour
{
    public float Force;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(TRTools.Helpers.TryFind<IKnockbackable>(other.gameObject, out IKnockbackable knockbackable))
        {
            knockbackable.GetKnockedBack(Force*Time.deltaTime*transform.forward);
        }
    }
}
