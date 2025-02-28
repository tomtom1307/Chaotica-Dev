using System.Collections.Generic;
using UnityEngine;

public class EnemyPerception : MonoBehaviour
{
    public EnemyBrain brain;
    Transform player;


    public bool LOS;
    public float Distance;


    //Get reference to player
    private void Start()
    {
        player = GameManager.instance.player;
    }



    //Updates variables every frame 
    public void UpdateFrameVariables(List<string> functions)
    {
        foreach (string func in functions) Invoke(func, 0);
    }

    //triggers a repeating check every t second
    public void StartRepeatingChecks(List<string> functions, List<float> times)
    {
        Debug.Log("DidRepeating check");
        for (int i = 0; i < functions.Count; i++)
        {
            Debug.Log(functions[i]);
            InvokeRepeating(functions[i], 0, times[i]);
        }
    }


    public void StopRepeatingChecks() => CancelInvoke();

    //Shoots a raycast between the enemy and the player and if it is a direct hit return true
    public void CheckLOS() 
    {
        Debug.Log("DidLOSCheck!");
        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, player.position - transform.position + Vector3.up, out hit, brain.DetectionRange, brain.layerMask))
        {
            if (hit.collider.gameObject.layer == 10)
            {
                LOS = true;
                return;
            }
            LOS = false;
        }
        else
        {
            LOS = false;
        }
        

    }
    public float CheckPlayerDistance() 
    {
        return Vector3.Distance(transform.position, player.position);
    }

    public int CountEnemies() 
    {
        //Will do this when we have spawners or enemy manager script!
        Debug.LogError("CountEnemies method has not been implemented yet!");
        return 0;
    }
}
