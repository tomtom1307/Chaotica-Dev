using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using System;

public class EnemyPerception : MonoBehaviour
{
    public EnemyBrain brain;
    public Transform player;
    public Vector3 PlayerLastSeenPosition;

    public bool LOS;
    public bool IsAlert;

    public float Distance;
    public float LSP_time;
    public float DetectionMeter;
    public DetectionIndicator DI;
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
            InvokeRepeating(functions[i], 0, times[i]);
        }
    }


    public void StopRepeatingChecks() => CancelInvoke();


    //Shoots a raycast between the enemy and the player and if it is a direct hit return true
    public void CheckLOS() 
    {
        RaycastHit hit;
        //Vector3.up because enemy transform position is at feet
        Vector3 LookVector = Vector3.Project(brain.LookDirectionTransform.forward, transform.forward).normalized;
        Debug.DrawRay(brain.LookDirectionTransform.position, player.position - brain.LookDirectionTransform.position + Vector3.up, Color.blue);
        Debug.DrawRay(brain.LookDirectionTransform.position, LookVector*5, Color.red);
        if (Physics.Raycast(brain.LookDirectionTransform.position, player.position - brain.LookDirectionTransform.position + Vector3.up, out hit, brain.DetectionRange, brain.layerMask)) 
        {
            
            if (hit.collider.gameObject.layer == 10 && Vector3.Angle(LookVector, player.position - transform.position) < brain.ViewAngle)
            {
                if (!LOS)
                {
                    
                }
                LOS = true;
                if(DI ==  null)
                {
                   DI = HUDController.instance.TriggerDetectionMeter(brain);
                }
                PlayerLastSeenPosition = player.transform.position;
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
        CheckLOS();
        float ModifyAmount;

        if (LOS)
        {
            ModifyAmount = 0.035f * Mathf.Exp(-((1/3)*Distance-5)) * brain.PerceptionStat * Time.deltaTime;
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
        CheckLOS();
        LSP_time = Time.time - LastSpottedTime;
    }

    public int CountEnemies() 
    {
        //Will do this when we have spawners or enemy manager script!
        Debug.LogError("CountEnemies method has not been implemented yet!");
        return 0;
    }
}
