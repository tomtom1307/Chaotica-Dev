using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalInteract : MonoBehaviour
{
    

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        
    }

    public void LoadScene(string SceneName)
    {
        SceneLoader.instance.LoadScene(SceneName);
    }

}
