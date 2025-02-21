using UnityEngine;

public class StatUpgradeUI : MonoBehaviour
{

    public GameObject Canvas;

    public void _EnterUpgradeWindow()
    {
        UIManager.instance.IsMenu(true, Canvas);
        UIManager.instance.ShowCursor();
    }

}
