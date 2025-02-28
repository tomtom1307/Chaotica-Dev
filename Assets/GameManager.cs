using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created]

    public static GameManager instance;

    public Transform player;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        player = GameObject.FindWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
