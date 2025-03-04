using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyPerception : MonoBehaviour
{
    public EnemyBrain brain;
    Transform player;
    Vector3 PlayerLastSeenPosition;

    public bool LOS;
    public bool IsAlert;

    public float Distance;
    public float LSP_time;
    public float DetectionMeter;

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
        RaycastHit hit;
        //Vector3.up because enemy transform position is at feet
        if(Physics.Raycast(transform.position + Vector3.up, player.position - transform.position + Vector3.up, out hit, brain.DetectionRange, brain.layerMask))
        {
            if (hit.collider.gameObject.layer == 10 && Vector3.Angle(transform.forward, player.position - transform.position + Vector3.up) < brain.ViewAngle)
            {
                LOS = true;
                LastSpottedTime = Time.time;
                return;
            }
            LOS = false;
        }
        else
        {
            LOS = false;
        }
    }





    public void CheckPlayerDistance() 
    {
        Distance = Vector3.Distance(transform.position, player.position);
    }


    public float TimeSinceCheckedDetection;

    public void CheckDetectionMeter()
    {
        float ModifyAmount;

        if (LOS)
        {
            ModifyAmount = brain.PerceptionStat* Time.deltaTime;
        }
        else
        {
            ModifyAmount = - brain.DetectionMeterDecay * Time.deltaTime;
        }

        DetectionMeter += ModifyAmount;
        DetectionMeter = Mathf.Clamp01(DetectionMeter);
    }



    float LastSpottedTime;
    public void CheckLastSeenPlayerTime()
    {   
        LSP_time = Time.time - LastSpottedTime;
    }

    public int CountEnemies() 
    {
        //Will do this when we have spawners or enemy manager script!
        Debug.LogError("CountEnemies method has not been implemented yet!");
        return 0;
    }
}
