using UnityEngine;

[CreateAssetMenu(fileName = "SloMo", menuName = "Abilities/SloMo")]
public class SloMo : Ability
{
    public float SlowPercent;
    

    public override void Activate(GameObject parent)
    {
        GameManager.instance.SlowTimeScale(SlowPercent, activeTime);
    }
}
