using UnityEngine;

public class WeaponStash : MonoBehaviour
{
    public GameObject canvas;

    public void OpenWeaponStashMenu()
    {
        UIManager.instance.IsMenu(true, canvas);
        UIManager.instance.ShowCursor();
    }



}
