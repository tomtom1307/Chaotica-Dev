using Unity.Jobs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AbilityUIController : MonoBehaviour
{
    public GameObject Player;

    public VisualElement ui;

    public Button JumpAbilityButton;
    private void Awake()
    {
        // Takes from UI document that the script is on

        ui = GetComponent<UIDocument>().rootVisualElement;

        
    }
    private void OnEnable()
    {
        //  

        JumpAbilityButton = ui.Q<Button>("JumpAbility");
        JumpAbilityButton.clicked += OnJumpAbilityButtonClicked;

        print("imenabled");
    }

    private void OnDisable()
    {
        JumpAbilityButton.clicked -= OnJumpAbilityButtonClicked;

        print("imdiscbaale");
    }
    private void OnJumpAbilityButtonClicked()
    {
        if (Player.GetComponent<AbilityHolder>().enabled == false)
        {
            // Activates Jump ability on Character
            print("Hey itss enabled");

            Player.GetComponent<AbilityHolder>().enabled = true;
        } else
        {
            // Disables the Jump ability on Character
            Debug.Log("Hey it is now disabled");

            Player.GetComponent<AbilityHolder>().enabled = false;
        }

        print("Buttonpressedyo");
    }
}
