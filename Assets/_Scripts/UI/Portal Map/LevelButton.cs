using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] public LevelData levelData;
    private GameObject textBox;

    public void InitializeData(LevelData levelData)
    {
        this.textBox = this.transform.GetChild(0).gameObject;
        textBox.SetActive(false);
        textBox.GetComponent<TextMeshProUGUI>().text = levelData.name;
        this.levelData = levelData;
    }

    public void LoadLevel()
    {
        // Close Menu and Load Scene associated with level
        if (levelData != null) 
        { 
            UIManager.instance.CloseCurrentMenu(); 
            SceneLoader.instance.LoadScene(levelData.sceneName); 
        }
    }
    public void LevelInfo()
    {
        // Render Level info dialog
        if (levelData != null) RenderInfoDialogue();
        else Debug.Log("Level Data not initialized");
    }
    private void RenderInfoDialogue()
    {
        //Logic for displaying info saved in levelData.levelDescription
        textBox.SetActive(true);
    }
    public void CloseInfo()
    {
        if (levelData != null) DestroyDialogue();
    }
    private void DestroyDialogue()
    {
        textBox.SetActive(false);
    }



}
