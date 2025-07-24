using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 dir;
    public float speed;
    public float Damage;
    public float AntiGrav;
    public bool Crit;
    public LayerMask LayerMask;
    public float MaxLifeTime = 30;
    Rigidbody rb;
    public float KnockBack;

    Collider col; 


    public virtual void Start()
    {
        col = GetComponent<Collider>();
        Destroy(gameObject, MaxLifeTime);
    }

    public virtual void Init(Vector3 dir, float speed, float Damage, bool gravity, float KnockBack = 0, bool crit = false)
    {
        Crit = crit;
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
        this.KnockBack = KnockBack;
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
            OnHit(collision.gameObject, collision.GetContact(0), Crit, collision);
        }

        rb.isKinematic = true;
        this.col.enabled = false;

        TrailRenderer TR = GetComponentInChildren<TrailRenderer>();
        if (TR != null)
        {
            TR.transform.parent = null;
            Destroy(TR.gameObject, 4);
        }
    }


    public virtual void OnHit(GameObject hitObj, ContactPoint col, bool isCrit, Collision collision)
    {
        if (hitObj.tag == "Head")
        {
            isCrit = true;
            hitObj = hitObj.GetComponentInParent<Damagable>().gameObject;
            Damage *= PlayerStats.instance.GetStat(StatType.CritMultiplier);
        }

        Damagable damagable = Damagable.CheckForDamagable(hitObj);

        //Knockback on enemy by projectile -> no worky

        Weapon_Attack_Data_Base.TryEnablingPhysics(damagable);

        Rigidbody hitrb;

        if (damagable.TryGetComponent<Rigidbody>(out hitrb))
        {
            hitrb.AddForceAtPosition(KnockBack * -collision.relativeVelocity.normalized, collision.GetContact(0).point);
        }
        

        //Damage pipeline

        damagable.TakeDamage(Damage, col.point, col.normal, isCrit);

        Destroy(gameObject);
    }
}
