using UnityEngine;

public class DisableOnLoad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObject.activeInHierarchy == false) enabled = false;
        else
        {
            gameObject.SetActive(false);
        }
        
    }

}
