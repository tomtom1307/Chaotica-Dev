using UnityEngine;

[CreateAssetMenu(menuName = "TomeUpgrade/StatUpgrade")]
public class TomeUpgradeStats : TomeUpgradeSOBase
{

    public bool StatUpgrade;
    public StatType Stat;
    public float StatIncrease;

    public override void Upgrade(PlayerStats Player)
    {
        base.Upgrade(Player);
        Player.ModifyStat(Stat, StatIncrease);
    }

}
