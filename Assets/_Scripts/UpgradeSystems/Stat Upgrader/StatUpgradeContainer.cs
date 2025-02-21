using TMPro;
using UnityEngine;

public class StatUpgradeContainer : MonoBehaviour
{
    public string StatName;
    public StatType StatType;
    //public string Description;
    float currentValue;
    public Sprite Icon;
    public float increments;
    public int price;
    public int priceIncrement;
    [SerializeField] int TimesLeveled;


    [SerializeField] TMP_Text NameTxt;
    [SerializeField] TMP_Text NextValueTxt;
    [SerializeField] TMP_Text CurrentValueTxt;
    [SerializeField] TMP_Text PriceTxt;

    [HideInInspector] public StatUpgradeUI UpgradeStore;


    public void Initialize()
    {
        UpdateUIValues(PlayerStats.instance.GetStat(StatType));

    }

    public void UpdateUIValues(float value)
    {
        currentValue = value;
        TimesLeveled = (int)(currentValue % increments);

        NameTxt.text = StatName;
        NextValueTxt.text = $"+{currentValue + increments}%";
        CurrentValueTxt.text = $"+{currentValue}%";
        PriceTxt.text = $"{price} CC";
    }

    public void Upgrade()
    {
        //Check Currency
        if(UpgradeStore.PlayerInventory.ChaosCores >= price){
            UpgradeStore.PlayerInventory.ChaosCores -= price;
            UpgradeStore.GetCurrencyValue();
            TimesLeveled++;
            currentValue += increments;
            price += priceIncrement;
            UpdateUIValues(currentValue);
            PlayerStats.instance.SetStat(StatType, currentValue);
        }

        

        //Else Deny :TODO show Tool tip or animate Purchase Button to wiggle or somet

    }





}
