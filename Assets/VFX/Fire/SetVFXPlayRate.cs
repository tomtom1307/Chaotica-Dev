using UnityEngine;
using UnityEngine.VFX;

public class SetVFXPlayRate : MonoBehaviour
{
    public float playRate = 1;


    private void Start()
    {
        GetComponent<VisualEffect>().playRate = playRate;
    }
}
