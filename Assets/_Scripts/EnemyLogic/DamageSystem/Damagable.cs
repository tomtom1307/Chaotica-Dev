using NUnit.Framework;
using Project;
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
    public Transform targetPos;
    GenericSoundManager soundManager;

    public Transform GetTargetPos()
    {
        if (targetPos == null) return this.transform;
        else return targetPos;  
    }

    protected virtual void Start()
    {
        soundManager = GetComponent<GenericSoundManager>();
        DamageNumber = Resources.Load<GameObject>("DamageNumber");
        Health = MaxHealth;
        
    }


    public virtual void TakeDamage(float damage, bool HitFX = true, bool crit = false)
    {
        if (ded == true) return;
        Health -= damage;
        if (soundManager != null) soundManager.PlaySound(0);
        if(HitFX) OnHitSpawn();

      
        if (crit) OnDamageTaken(damage, DamageTypeDisplay.Crit);
        else OnDamageTaken(damage, DamageTypeDisplay.Standard);

        if (Health <= 0)
        {
            Die();
        }
    }

    public virtual void TakeDamage(float Damage, Vector3 pos, Vector3 normal, bool isCrit)
    {
        TakeDamage(Damage, false, isCrit);
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
        CamShake.instance.StartShake(CamShake.instance.onDestroy);
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

    public virtual void OnDamageTaken(float damage, DamageTypeDisplay damageType)
    {
        if (DamageNumbers)
        {
            DamagePopupGenerator.Instance.CreateDamageNumber(damage, transform.position,damageType);
        }
    }




    // Update is called once per frame
    protected virtual void Update()
    {
        
    }


    public static Damagable CheckForDamagable(GameObject go)
    {
        Damagable D = go.GetComponent<Damagable>();
        if (D== null)
        {
            D = go.GetComponentInParent<Damagable>();
        }
        return D;
    }

    
}

