using UnityEngine;

public class TomeUpgradeSOBase : ScriptableObject
{
    public Sprite Display;
    public string UpgradeTitle;

    [TextArea(10,100)]
    public string UpgradeDescription;


    public virtual void Upgrade(PlayerStats Player)
    {
        
    }
}
