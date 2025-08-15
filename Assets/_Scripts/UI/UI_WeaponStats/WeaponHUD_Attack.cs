using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD_Attack : MonoBehaviour
{
    public TMP_Text DamageAmount;
    public Image IconImage;


    [Header("Icons")]
    public Sprite MeleeIcon;
    public Sprite Projectile;
    public Sprite AOE;
    public Sprite Raycast;
    public Sprite BlockParry;
    public Sprite DefaultIcon;



    public void PopulateAttackData(Weapon_Attack_Data_Base attackData)
    {
        DamageAmount.SetText(attackData.damage.ToString() + "%");
        IconImage.sprite = GetIconForType(attackData.attackType);
    }

    Sprite GetIconForType(AttackType type)
    {
        switch (type)
        {
            case AttackType.Melee: return MeleeIcon;
            case AttackType.Raycast: return Raycast;
            case AttackType.AOE: return AOE;
            case AttackType.Projectile: return Projectile;
            case AttackType.BlockParry: return BlockParry;
            default: return DefaultIcon;
        }
    }

}
