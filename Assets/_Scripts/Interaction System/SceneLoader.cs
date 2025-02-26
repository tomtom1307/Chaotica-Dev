using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image progressBar;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(string SceneName) 
    { 
        var scene  = SceneManager.LoadSceneAsync(SceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do
        {


            progressBar.fillAmount = scene.progress;
        } while (scene.progress < 0.9);

        scene.allowSceneActivation = true;

        _loaderCanvas.SetActive(false);

    }

}
