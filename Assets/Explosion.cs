using Project;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public float Range;
    public float Damage;
    public float Force;
    public float UpForce;
    public CamShake.CamShakeProperties camShakeProperties;
    AudioSource As;
    void Start()
    {
        As = GetComponent<AudioSource>();
        if(audioClips.Count > 0) { As.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]); }
        CamShake.instance.StartShake(camShakeProperties);
        
        List<Collider> colliders = Physics.OverlapSphere(transform.position, Range).ToList();

        foreach(Collider collider in colliders)
        {
            Rigidbody rb;
            rb = collider.GetComponent<Rigidbody>();
            if(rb == null) rb = collider.GetComponentInChildren<Rigidbody>();
            if (rb == null) continue;


            if (rb.CompareTag("Enemy"))
            {
                EnemyBrain Eb = rb.GetComponent<EnemyBrain>();
                if(Eb != null)
                    Eb.TogglePhysics(true);
            }

            Damagable damagable;
            if(rb.TryGetComponent<Damagable>(out damagable))
            {
                damagable.TakeDamage(Damage);
            }

            
            rb.AddExplosionForce(Force, transform.position, Range, UpForce);

        }

    }

}
