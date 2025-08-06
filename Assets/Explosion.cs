using Project;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public float Range;
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
            if(collider.TryGetComponent<Rigidbody>(out rb))
            {
                rb.AddExplosionForce(Force, transform.position, Range, UpForce);
            }
        }

    }

}
