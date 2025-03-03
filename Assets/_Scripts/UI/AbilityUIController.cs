using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AbilityUIController : MonoBehaviour
{
    public VisualElement ui;

    public Button SpawnPoopButton;
    private void Awake()
    {
        // Takes from UI document that the script is on

        ui = GetComponent<UIDocument>().rootVisualElement;
    }
    private void OnEnable()
    {
        //  

        SpawnPoopButton = ui.Q<Button>("SpawnPoop");
        SpawnPoopButton.clicked += OnSpawnPoopButtonClicked;
    }
    private void OnSpawnPoopButtonClicked()
    {
        // Spawns poop on hand (idk) and gives damage boost to player

        Debug.Log("yay you can spawn poop now");
    }
}
