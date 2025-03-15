using TMPro;
using UnityEngine;

public class WeaponAttackItem : MonoBehaviour
{
    public int AttackNum;
    public GameObject Obscurer;
    public TMP_Text AttackName;
    public TMP_Text Description;
    public TMP_Text Damage;
    public TMP_Text KillRequirement;

    public void FillInfo(WeaponInstance Inst)
    {
        AttackName.text = Inst.data.Weapon_Attacks[AttackNum].Name;
        Description.text = Inst.data.Weapon_Attacks[AttackNum].Description;
        Damage.text = Inst.data.Weapon_Attacks[AttackNum].damage + "%";

        Obscurer.SetActive(!IsAttackUnlocked(Inst));
    }

    public bool IsAttackUnlocked(WeaponInstance Inst)
    {
        if (AttackNum == 0)
            return true;

        if(AttackNum == 1)
        {
            if(Inst.KillCount >= Inst.Threshold1)
            {
                return true;
            }
            else
            {
                KillRequirement.text = Inst.Threshold1 - Inst.KillCount + "Kills to Unlock";
                return false;
            }
        }

        if (AttackNum == 2)
        {
            if (Inst.KillCount == Inst.Threshold2)
            {
                return true;
            }
            else
            {
                KillRequirement.text = Inst.Threshold2 - Inst.KillCount + "Kills to Unlock";
                return false;
            }
        }
        else
        {
            KillRequirement.text = "Somet Did Poopy";
            return false;
        }
    }

    
}
