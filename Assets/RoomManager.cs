using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    List<DoorLogic> Doors;
    public List<DoorLogic> ProgressionDoors; //Stores doors that lead to another room. 
    bool Started = false;


    void Start()
    {
        Doors = GetComponentsInChildren<DoorLogic>().ToList();
    }

    void Update()
    {
        
    }
    
    public void CloseAllDoors()
    {
        foreach (DoorLogic door in Doors)
        {
            door.ToggleDoor(false);
        }
    }

    public void OpenProgressionDoors()
    {
        foreach (DoorLogic door in ProgressionDoors)
        {
            door.ToggleDoor(true);
        }
    }


    public void StartRoom()
    {
        Started = true;
        CloseAllDoors();
        GetComponent<RoomSpawner>().TriggerSpawner();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10 && !Started)
        {
            StartRoom();
        }
    }

    public void RoomComplete()
    {
        OpenProgressionDoors();
    }


}
