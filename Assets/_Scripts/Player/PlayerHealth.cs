using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image UIHealthBarImage; 


    [SerializeField] float health;
    float maxHealth;

    PlayerStats stats;

    private void Start()
    {
        //GetStats 
        stats = GetComponent<PlayerStats>();
        
        GetMaxHealth();
        health = maxHealth;
    }

    private void Update()
    {

        //Testing
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(5);
        }
    }

    

    public void GetMaxHealth()
    {
        //Has MaxHealth increase for when upgrades have been done
        maxHealth = stats.MaxHealth * (1 + stats.MaxHealthIncrease);

    }

    //Take damage with UI handling
    public void TakeDamage(float Amount)
    {
        health -= Amount;
        UIHealthBarImage.fillAmount = health / maxHealth;
        if (health <= 0)
        {
            Die();
        }
        
    }


    public void Die()
    {
        Debug.Log("You Have Died!");
    }



}
