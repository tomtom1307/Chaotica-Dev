using UnityEngine;

public class Damagable : MonoBehaviour
{
    public float MaxHealth = 10;
    public float Health;

    public bool DamageNumbers = true;
    [HideInInspector] GameObject DamageNumber;
    public GameObject SpawnOnDeath;
    public Vector3 spawnOffset;
    public GameObject model;
    public bool ded;

    protected virtual void Start()
    {
        DamageNumber = Resources.Load<GameObject>("DamageNumber");
        Health = MaxHealth; 
    }


    public void TakeDamage(float damage)
    {
        if (ded == true) return;
        Health -= damage;
        OnDamageTaken(damage);
        if(Health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if(ded == true) { return; }
        ded = true;
        if(SpawnOnDeath != null)
        {
            Instantiate(SpawnOnDeath, transform.position, transform.rotation);
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


    public void SpawnDamageNumbers(float damage)
    {
        Vector3 offsetVector = spawnOffset.x * transform.right + spawnOffset.y * transform.up + spawnOffset.z * transform.forward;
        GameObject number = Instantiate(DamageNumber,offsetVector+ transform.position+0.3f*(Camera.main.transform.position-transform.position).normalized, Quaternion.identity);
        number.GetComponent<DamageNumber>().SetValue(damage);
    }




    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
