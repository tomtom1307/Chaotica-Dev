using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TomeUpgradePannel : MonoBehaviour
{
    [HideInInspector]public TomeUpgradeSOBase upgrade;
    GameObject player;
    GameObject TomeGO;
    public TMP_Text Title;
    public TMP_Text Description;
    public Image image;

    public void PopulateInfo(TomeUpgradeSOBase u, UpgradeTome UT)
    {
        upgrade = u;
        TomeGO = UT.gameObject;
        player = UT.player;
        Title.text = u.UpgradeTitle;
        Description.text = u.UpgradeDescription;
        if(u.Display != null)
        {
            image.sprite = u.Display;
        }
    }

    public void DoUpgrade()
    {
        upgrade.Upgrade(player.GetComponent<PlayerStats>());
        UIManager.instance.IsMenu(false);
        UIManager.instance.LockCursor();
        Destroy(TomeGO);
    }

}
