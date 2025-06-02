using DG.Tweening;
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

    public void LoadScene(string SceneName) 
    {
        ClosingProcesses();
        SceneManager.LoadScene(SceneName);
    }

    // Necessary processes for closing a scene
    private void ClosingProcesses()
    {
        //Stop tween errors when objects are unloaded.
        DOTween.KillAll();
    }

}
