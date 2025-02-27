using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public VisualElement ui;

    public Button playButton;
    public Button quitButton;
    private void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void Start()
    {
        UIManager.instance.IsMenu(true, this.gameObject);
    }
    private void OnEnable()
    {
        playButton = ui.Q<Button>("Play");
        playButton.clicked += OnPlayButtonClicked;

        quitButton = ui.Q<Button>("Quit");
        quitButton.clicked += OnQuitButtonClicked;
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    private void OnPlayButtonClicked()
    {
        gameObject.SetActive(false);

        UIManager.instance.IsMenu(false, this.gameObject);
    }

}