using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 dir;
    public float speed;
    public float Damage;
    public LayerMask LayerMask;
    public float MaxLifeTime = 30;
    Rigidbody rb;



    public virtual void Start()
    {
        Destroy(gameObject, MaxLifeTime);
    }

    public virtual void Init(Vector3 dir, float speed, float Damage, bool gravity)
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = dir * speed;
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
                transform.LookAt(rb.linearVelocity);
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
    }


    public virtual void OnHit(GameObject hitObj)
    {
        hitObj.GetComponent<Damagable>().TakeDamage(Damage);
        Destroy(gameObject);
    }
}
