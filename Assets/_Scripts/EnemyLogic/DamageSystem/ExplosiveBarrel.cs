using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class ExplosiveBarrel : Damagable
{
    [SerializeField] private float deathDelay = 0.75f;
    private bool _dying;
    public GameObject Fire;

    protected override void Start()
    {
        base.Start();
        Fire.SetActive(false);
    }

    public override void Die()
    {
        if (_dying || ded) return;  // double-tap protection

        _dying = true;           // mark dead NOW so it stops taking damage/FX
        Fire.SetActive(true);
        Fire.GetComponent<VisualEffect>().enabled = true;
        Fire.GetComponent<LookUp>().enabled = true;
        // Make inert so it can’t trigger/receive more chaos during the delay
        
        var rb = GetComponent<Rigidbody>();

        StartCoroutine(DieAfter(deathDelay));
    }

    private IEnumerator DieAfter(float seconds)
    {
        if (seconds < 0f || float.IsNaN(seconds)) seconds = 0f;
        yield return new WaitForSeconds(seconds);

        base.Die(); // runs your spawn-on-death, cam shake, destroy, etc.
    }
}
