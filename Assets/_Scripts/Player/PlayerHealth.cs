using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image UIHealthBarImage; 


    [SerializeField] float health;

    float maxHealth { get { return stats.GetStat(StatType.MaxHealth) * (1 + 0.01f * stats.GetStat(StatType.MaxHealthIncrease)); } }

    PlayerStats stats;
    FullScreenFXController fullScreenFX;
    private void Start()
    {
        //GetStats 
        stats = PlayerStats.instance;
        health = maxHealth;
        fullScreenFX = GetComponentInParent<FullScreenFXController>();
    }

    private void Update()
    {
        UpdateHealthBar();
        //Testing
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(5);
        }
        if(Input.GetKeyDown(KeyCode.H)) {

            TakeDamage(-10);
        }
    }

    

    

    //Take damage with UI handling
    public void TakeDamage(float Amount)
    {
        health -= Amount;
        UpdateHealthBar();
        fullScreenFX.currentHurtCorutine = StartCoroutine(fullScreenFX.Hurt());
        if (health <= 0)
        {
            Die();
        }
    }


    public void UpdateHealthBar() {
        UIHealthBarImage.fillAmount = health / maxHealth;
    }


    public void Die()
    {
        Debug.Log("You Have Died!");
    }



}
