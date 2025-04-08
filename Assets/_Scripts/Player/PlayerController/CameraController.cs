using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [SerializeField] private float LerpSpeed;
    [SerializeField] public Transform orientation;
    public GameObject Player;
    [SerializeField] Transform HeadPos;

    Camera cam;

    float mousex;
    float mousey;

    float multiplier = 0.01f;

    float xRot;
    float yRot;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        myInput();

        cam.transform.localRotation = Quaternion.Euler(xRot, yRot, HeadPos.eulerAngles.z);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);

        transform.position = HeadPos.position;


    }

    public void ChangeSens(float val)
    {
        sensX = val;
        sensY = val;
    }

    [HideInInspector] public Vector2 mouseInput;
    void myInput()
    {
        mousex = Input.GetAxisRaw("Mouse X");
        mousey = Input.GetAxisRaw("Mouse Y");
        mouseInput.x = mousex;
        mouseInput.y = mousey;


        yRot += mousex * sensX * multiplier;
        xRot -= mousey * sensY * multiplier;

        xRot = Mathf.Clamp(xRot, -90, 90);

    }

}
