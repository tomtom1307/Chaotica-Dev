using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public float MaxHealth = 10;
    public float Health;

    public bool DamageNumbers = true;
    [HideInInspector] GameObject DamageNumber;
    public List<GameObject> SpawnOnDeath;
    public List<GameObject> SpawnOnHit;
    public Vector3 spawnOffset;
    public GameObject model;
    public bool ded;

    GenericSoundManager soundManager;

    protected virtual void Start()
    {
        soundManager = GetComponent<GenericSoundManager>();
        DamageNumber = Resources.Load<GameObject>("DamageNumber");
        Health = MaxHealth; 
    }


    public virtual void TakeDamage(float damage, bool HitFX = true)
    {
        if (ded == true) return;
        Health -= damage;
        if (soundManager != null) soundManager.PlaySound(0);
        if(HitFX) OnHitSpawn();
        OnDamageTaken(damage);
        if(Health <= 0)
        {
            Die();
        }
    }

    public virtual void TakeDamage(float Damage, Vector3 pos, Vector3 normal)
    {
        TakeDamage(Damage, false);
        OnHitSpawn(pos, normal);

    }

    public virtual void OnHitSpawn()
    {
        foreach (GameObject obj in SpawnOnHit)
        {
            if (obj != null)
            {
                Instantiate(obj, transform.position, transform.rotation);
            }
        }
    }


    public virtual void OnHitSpawn(Vector3 pos, Vector3 normal)
    {
        foreach (GameObject obj in SpawnOnHit)
        {
            if (obj != null)
            {
                Instantiate(obj, pos, Quaternion.LookRotation(normal));
            }
        }
    }
    



    public virtual void Die()
    {
        if(ded == true) { return; }
        ded = true;

        if (soundManager != null) soundManager.PlaySound(1);

        if (SpawnOnDeath.Count > 0)
        {
            foreach(GameObject obj in SpawnOnDeath)
            {
                if(obj != null)
                {
                    Instantiate(obj, transform.position, transform.rotation);
                }
                
            }
        }
        if(model != null)
        {
            model.SetActive(false);
        }
        Destroy(gameObject,0.5f);
        
    }

    public virtual void OnDamageTaken(float damage)
    {
        if(DamageNumbers)
        {
            SpawnDamageNumbers(damage);
        }
    }


    public virtual void SpawnDamageNumbers(float damage)
    {
        Vector3 offsetVector = spawnOffset.x * transform.right + spawnOffset.y * transform.up + spawnOffset.z * transform.forward;
        GameObject number = Instantiate(DamageNumber,offsetVector+ transform.position+0.3f*(Camera.main.transform.position-transform.position).normalized, Quaternion.identity);
        number.GetComponent<DamageNumber>().SetValue(damage);
    }

    public virtual void SpawnDamageNumbers(float damage, Vector3 HitPos)
    {
        Vector3 offsetVector = spawnOffset.x * transform.right + spawnOffset.y * transform.up + spawnOffset.z * transform.forward;
        GameObject number = Instantiate(DamageNumber, offsetVector + transform.position + 0.3f * (Camera.main.transform.position - transform.position).normalized, Quaternion.identity);
        number.GetComponent<DamageNumber>().SetValue(damage);
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
