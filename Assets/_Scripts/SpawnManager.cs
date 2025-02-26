using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Debug.Log("MovedPlayer");
            player.GetComponent<Rigidbody>().MovePosition(transform.position) ;
            player.transform.rotation = transform.rotation;
        }
        else
        {
            Debug.Log("Player was not found");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {

            player.GetComponent<Rigidbody>().MovePosition(transform.position);
            player.transform.rotation = transform.rotation;
        }
        else
        {
            Debug.Log("Player was not found");
        }
    }
}
