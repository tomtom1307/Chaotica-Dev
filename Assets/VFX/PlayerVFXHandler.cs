using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXHandler : MonoBehaviour
{
    public GameObject VFX;
    public List<Transform> VFXHolders;
    public static PlayerVFXHandler instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha4))
        {
            var GO = Instantiate(VFX, VFXHolders[0]);
            GO.GetComponentInChildren<VisualEffect>().Play();
            Destroy(GO, 5);
        }
    }

    public void SpawnVFX(GameObject VFX, int i)
    {
        var GO = Instantiate(VFX, VFXHolders[i]);
        GO.GetComponentInChildren<VisualEffect>().Play();
        Destroy(GO, 5);
    }

}
