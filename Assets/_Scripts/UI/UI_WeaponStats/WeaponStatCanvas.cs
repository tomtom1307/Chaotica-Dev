using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatCanvas : MonoBehaviour
{
    public float AnimationSmoothing;
    public float DisplayDelay;
    public GameObject WeaponStatHolder;
    public static WeaponStatCanvas instance;

    [SerializeField] TMP_Text w_name;
    [SerializeField] TMP_Text BaseDamage;
    [SerializeField] TMP_Text DPS;
    [SerializeField] TMP_Text Handedness;
    [SerializeField] Image Sprite;

    WeaponHUDAttackManager attackManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        attackManager = GetComponentInChildren<WeaponHUDAttackManager>();
        HideWeaponStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateData(WeaponInstance WI)
    {
        w_name.text = WI.data.name;
        BaseDamage.text = WI.data.WeaponDamage.ToString();
        DPS.text = "5f";
        Sprite.sprite = WI.data.InventorySprite;

        attackManager.PopulateAttacks(WI);

    }

    public void DisplayWeaponStats()
    {
        WeaponStatHolder.transform.DOScale(1, AnimationSmoothing).SetDelay(DisplayDelay);
    }

    public void HideWeaponStats()
    {
        WeaponStatHolder.transform.DOScale(0, AnimationSmoothing);
    }


}
