using UnityEngine;

public class Damagable : MonoBehaviour
{
    public float MaxHealth;
    public float Health;

    public bool DamageNumbers;
    [HideInInspector] GameObject DamageNumber;





    protected virtual void Start()
    {
        DamageNumber = Resources.Load<GameObject>("DamageNumber");
        Health = MaxHealth; 
    }


    public void TakeDamage(float damage)
    {
        Health -= damage;
        OnDamageTaken(damage);
        if(Health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
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
        GameObject number = Instantiate(DamageNumber, transform.position+0.3f*(Camera.main.transform.position-transform.position).normalized, Quaternion.identity);
        number.GetComponent<DamageNumber>().SetValue(damage);
        
    }




    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
