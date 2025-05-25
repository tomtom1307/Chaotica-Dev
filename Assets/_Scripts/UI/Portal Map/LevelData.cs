using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public string sceneName;
    public Sprite levelSprite;
    public string levelDescription;
}
