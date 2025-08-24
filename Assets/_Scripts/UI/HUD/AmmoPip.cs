using UnityEngine;
using UnityEngine.UI;

public class AmmoPip : MonoBehaviour
{
    public Image Fill;
    public Image Ready;

    public void SetPipSprite()
    {

    }

    public void AmmoUsed()
    {
        Ready.gameObject.SetActive(false);
    }

    public void AmmoReady()
    {
        Ready.gameObject.SetActive(true);
    }

    public void SetFill(float amt)
    {
        Fill.fillAmount = amt;
    }


}
