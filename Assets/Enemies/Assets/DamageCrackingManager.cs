using UnityEngine;

public class DamageCrackingManager : MonoBehaviour
{
    Renderer renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TryGetComponent<Renderer>(out renderer);
        if (renderer == null) Destroy(this);
    }

    public void ChangeCrack(float percent)
    {
        if (renderer != null) 
        {
            renderer.material.SetFloat("_CrackVal", percent);
        }
    }
}
