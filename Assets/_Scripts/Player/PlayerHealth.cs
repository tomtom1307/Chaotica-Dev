  using Project;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image UIHealthBarImage; 
    public Image UIHealthEaseBarImage;

    [SerializeField ]private float lerpspeed1 = 0.01f;
    [SerializeField ]private float lerpspeed2 = 0.01f;

    [SerializeField] float health;

    float maxHealth { get { return stats.GetStat(StatType.MaxHealth) * (1 + 0.01f * stats.GetStat(StatType.MaxHealthIncrease)); } }

    PlayerStats stats;
    FullScreenFXController fullScreenFX;
    public static PlayerHealth instance;
    WeaponHolder weaponHolder;
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
        weaponHolder = GetComponent<WeaponHolder>();
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

    public void Heal(float Amount)
    {
        health = Mathf.Clamp(health + Amount, 0, maxHealth);
        //OtherStuff like SFX + visuals
    }

    //Take damage with UI handling
    public void TakeDamage(float Amount, EnemyBrain EB = null, bool paryable = false, bool blockable = false)
    {
        switch (d_state)
        {
            case (DamageState.Normal):
            {
                health -= Amount;
                UpdateHealthBar();
                CamShake.instance.StartShake(CamShake.instance.onHit);
                    PlayerSoundSource.instance.PlaySound(PlayerSoundSource.SoundType.TakeDamage);
                fullScreenFX.currentHurtCorutine = StartCoroutine(fullScreenFX.Hurt());
                if (health <= 0)
                    {
                        Die();
                    }
                break;
            }
            case (DamageState.Parrying):
            {
                if(EB == null || !paryable)
                {
                    d_state = DamageState.Normal;
                    TakeDamage(Amount);
                    break;
                }
                else if (paryable)
                {
                    Parry(EB);
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
        Weapon_Attack_Data_BlockParry weapon_Attack_Data_BlockParry = weaponHolder.CurrentAttackData as Weapon_Attack_Data_BlockParry;
        health -= amount * weapon_Attack_Data_BlockParry.DamageReduction;
        UpdateHealthBar();
        CamShake.instance.StartShake(CamShake.instance.onHit);
        fullScreenFX.currentHurtCorutine = StartCoroutine(fullScreenFX.Hurt());
        if (health <= 0)
        {
            Die();
        }
    }

    public GameObject ParryVFX;

    public void Parry(EnemyBrain EB)
    {
        PlayerSoundSource.instance.PlaySound(PlayerSoundSource.SoundType.Parry);
        Instantiate(ParryVFX, transform);
        EB.Stun();
    }

    public void UpdateHealthBar() {
        if(UIHealthBarImage.fillAmount != health/maxHealth) { UIHealthBarImage.fillAmount = Mathf.Lerp(UIHealthBarImage.fillAmount, health/maxHealth, lerpspeed1); }
        if(UIHealthBarImage.fillAmount != UIHealthEaseBarImage.fillAmount) { UIHealthEaseBarImage.fillAmount = Mathf.Lerp(UIHealthEaseBarImage.fillAmount, health/maxHealth, lerpspeed2); }
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
