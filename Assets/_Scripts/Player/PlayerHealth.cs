using Project;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image UIHealthBarImage; 


    [SerializeField] float health;

    float maxHealth { get { return stats.GetStat(StatType.MaxHealth) * (1 + 0.01f * stats.GetStat(StatType.MaxHealthIncrease)); } }

    PlayerStats stats;
    FullScreenFXController fullScreenFX;
    public static PlayerHealth instance;
    private void Start()
    {

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
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
            TakeDamage(5, null);
        }
        if(Input.GetKeyDown(KeyCode.H)) {

            TakeDamage(-10, null);
        }
    }

    public DamageState d_state;

    

    //Take damage with UI handling
    public void TakeDamage(float Amount, EnemyBrain EB = null)
    {
        switch (d_state)
        {
            case (DamageState.Normal):
            {
                health -= Amount;
                UpdateHealthBar();
                CamShake.instance.StartShake(CamShake.instance.onHit);
                fullScreenFX.currentHurtCorutine = StartCoroutine(fullScreenFX.Hurt());
                if (health <= 0)
                    {
                        Die();
                    }
                break;
            }
            case (DamageState.Parrying):
            {
                if(EB == null)
                    {
                        d_state = DamageState.Normal;
                        TakeDamage(Amount);
                    }
                break;
            }
            case (DamageState.Blocking):
            {
                BlockDamage(Amount);
                break;
            }
            case (DamageState.Invincible):
            {
                break;
            }
        }
        
        
    }

    public void BlockDamage(float amount)
    {

    }

    public void UpdateHealthBar() {
        UIHealthBarImage.fillAmount = health / maxHealth;
    }


    public void Die()
    {
        Debug.Log("You Have Died!");
    }


    public enum DamageState
    {
        Normal,
        Blocking,
        Parrying,
        Invincible
    }
}
