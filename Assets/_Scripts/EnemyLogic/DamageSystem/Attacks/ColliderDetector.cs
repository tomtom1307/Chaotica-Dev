using UnityEngine;
using System.Collections;
using System;

public class ColliderDetector : MonoBehaviour
{

    public Collider detectionCollider; 

    public Action<PlayerHealth, ColliderDetector> OnDetectCallback;
    public bool playerDetected = false; 
    private PlayerHealth ph;

    private void Start()
    {
        
        gameObject.SetActive(false);
        detectionCollider = GetComponent<Collider>();
        detectionCollider.isTrigger = true;
    }

    public void TriggerDetection() 
    {
        gameObject.SetActive(true);
        playerDetected = false;
    }


    public void DisableCollider()
    {
        playerDetected = true;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerDetected)
        {
            ph = other.gameObject.GetComponent<PlayerHealth>(); 
            if(ph != null )
            {
                PlayerDetected();//SendAction

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")&& !playerDetected)
        {
            ph = other.gameObject.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                //SendAction
                PlayerDetected();
            }
        }
    }


    

    private void PlayerDetected()
    {
        DisableCollider();
        OnDetectCallback?.Invoke(ph, this);

    }
}
