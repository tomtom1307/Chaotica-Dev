using DG.Tweening;
using UnityEngine;

public class DamagePopupGenerator : MonoBehaviour
{
    public static DamagePopupGenerator Instance;

    public Color Standard;
    public Color Crit;
    public Color Verdancy;
    public Color Scarforge;
    public Color Umbraveil;
    public Color Aetherflow;
    public Color Healing;
    public GameObject Number_prefab;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void CreateDamageNumber(float Value, Vector3 pos, DamageTypeDisplay damageType)
    {
        Color color = DetermineColor(damageType);
        GameObject Num = Instantiate(Number_prefab, pos, Quaternion.identity);
        Num.transform.localScale = Vector3.zero;
        Num.GetComponent<DamageNumber>().SetValue(Value, color);
    }

    public Color DetermineColor(DamageTypeDisplay damageType)
    {
        Color color;
        switch (damageType)
        {
            case DamageTypeDisplay.Standard:
                color = Standard;
                break;
            case DamageTypeDisplay.Crit:
                color = Crit;
                break;
            case DamageTypeDisplay.Verdancy:
                color = Verdancy;
                break;
            case DamageTypeDisplay.Scarforge:
                color = Scarforge;
                break;
            case DamageTypeDisplay.Umbraveil:
                color = Umbraveil;
                break;
            case DamageTypeDisplay.Aetherflow:
                color = Aetherflow;
                break;
            case DamageTypeDisplay.Healing:
                color = Healing;
                break;
            default:
                color = Standard;
                break;
        }
        return color;
    }
    
}

public enum DamageTypeDisplay
{
    Standard,
    Crit,
    Verdancy,
    Scarforge,
    Umbraveil,
    Aetherflow,
    Healing
}
