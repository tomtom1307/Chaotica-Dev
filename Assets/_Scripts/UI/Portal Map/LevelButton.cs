using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] public LevelData levelData;
    public void InitializeData(LevelData levelData)
    {
        this.levelData = levelData;
    }

    public void LoadLevel()
    {
        // Load Scene associated with level
        SceneLoader.instance.LoadScene(levelData.sceneName);
    }
    public void LevelInfo()
    {
        // Render Level info dialog
    }

}
