using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 dir;
    public float speed;
    public float Damage;
    public float AntiGrav;
    public LayerMask LayerMask;
    public float MaxLifeTime = 30;
    Rigidbody rb;

    Collider col; 


    public virtual void Start()
    {
        col = GetComponent<Collider>();
        Destroy(gameObject, MaxLifeTime);
    }

    public virtual void Init(Vector3 dir, float speed, float Damage, bool gravity)
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = dir * speed;
        transform.LookAt(transform.position + dir);
        if (gravity)
        {
            rb.useGravity = true;
            
        }
        else
        {
            rb.useGravity = false;
        }
        this.Damage = Damage;
    }


    public virtual void Update()
    {
        if(rb != null)
        {
            if (rb.linearVelocity.magnitude > 0.2f)
            {
                //Aligns Z direction with velocity
                transform.LookAt(transform.position + rb.linearVelocity);
                rb.AddForce(Vector3.up * AntiGrav, ForceMode.Acceleration);
            }
        }
    }

    bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((mask.value & (1 << obj.layer)) != 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(IsInLayerMask(collision.gameObject,LayerMask))
        {
            OnHit(collision.gameObject);
        }
        rb.isKinematic = true;
        col.enabled = false;


    }


    public virtual void OnHit(GameObject hitObj)
    {
        hitObj.GetComponent<Damagable>().TakeDamage(Damage);
        Destroy(gameObject);
    }
}
