using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD_ModifierPip : MonoBehaviour
{
    public Image PipSprite;

    public void SetSprite(Sprite sprite)
    {
        PipSprite.sprite = sprite;
    }
}
