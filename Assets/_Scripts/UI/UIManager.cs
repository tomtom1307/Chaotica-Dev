using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{

    public GameObject PauseMenu;


    bool inMenu;
    GameObject CurrentMenu;
    public static UIManager instance;
        
    PlayerMovement playerMovement;
    CameraController CamController;
    WeaponHolder weaponHolder;



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        playerMovement = GetComponent<PlayerMovement>();
        CamController = Camera.main.GetComponent<CameraController>();
        weaponHolder = GetComponent<WeaponHolder>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //If escape exit current menu
            if (inMenu)
            {
                if (CurrentMenu != null)
                {
                    IsMenu(false, CurrentMenu);
                    LockCursor();
                }
            }
            //Pause Game 
            else
            {
                ShowCursor();
                IsMenu(true, PauseMenu);
            }
        }
        

    }


    //Pass in boolean to establish if in menu pass in the MenuCanvas GO
    public void IsMenu(bool x, GameObject Canvas = null)
    {
        CurrentMenu = Canvas;

        if(CurrentMenu != null)
        {
            CurrentMenu.SetActive(x);
        }
        
        

        //Handle all disabling and enabling of components that need to be handled here
        inMenu = x;

        //Stop player from moving when in a menu
        ToggleControls(x);

        //TODO: Could pause Timescale????
    }


    public void ToggleControls(bool x)
    {
        playerMovement.enabled = !x;
        CamController.enabled = !x;
        weaponHolder.enabled = !x;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }





}
