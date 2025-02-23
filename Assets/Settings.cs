using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] Slider SensSlider;
    CameraController cam;


    private void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();
    }

    public void ChangeSens(float val)
    {
        cam.ChangeSens(val);
    }


}
