using UnityEngine;

public class DisableOnLoad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (gameObject.activeInHierarchy == false) enabled = false;
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        
    }

}
