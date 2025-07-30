using UnityEditor.Rendering;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public float Loops;
    public float Arenas;
    public float RoomSpacing;
    public GameObject Room;
    public GameObject Store;

    void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        //Stores current position
        Vector3 CurrentPos = Vector3.zero;


        for (int l = 0; l < Loops; l++)
        {
            // Spawn N number of fight arenas 
            for(int a = 0;  a < Arenas; a++)
            {
                Instantiate(Room, CurrentPos, Quaternion.identity);
                CurrentPos += RoomSpacing * Vector3.right;
            }

            //Then Spawn a shop
            Instantiate(Store, CurrentPos, Quaternion.identity);
            CurrentPos += RoomSpacing * Vector3.right;
        }
    }
}
