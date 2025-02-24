using UnityEngine;

public class Damagable : MonoBehaviour
{
    public float MaxHealth;
    public float Health;





    protected virtual void Start()
    {
        Health = MaxHealth; 
    }


    public void TakeDamage(float damage)
    {
        Health -= damage;
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
