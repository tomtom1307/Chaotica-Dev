using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created]

    public static GameManager instance;

    public List<EnemyPatrolPoint> patrolPoints;
    public List<EnemyPatrolPoint> availablePatrolPoints;


    public Transform player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        availablePatrolPoints = new List<EnemyPatrolPoint>();
        player = GameObject.FindWithTag("Player").transform;
    }

    
    

    public EnemyPatrolPoint FindClosestPatrol(Vector3 pos)
    {
        float minDist = 1000;
        EnemyPatrolPoint closestPP = null;
        foreach (var item in availablePatrolPoints)
        {
            float Dist = Vector3.Distance(item.transform.position, pos);
            if (minDist > Dist)
            {
                closestPP = item;
                minDist = Dist;
            }
        }
        if(closestPP == null)
        {
            Debug.LogError("NO AVAILABLE CLOSEST PATROL POINT FOUND");
        }
        return closestPP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SlowTimeScale(0.5f, 2);
        }
    }

    public void SlowTimeScale(float amount,float t)
    {
        Time.timeScale = amount;
        StartCoroutine(ResetTime(t));
    }

    public IEnumerator ResetTime(float t)
    {
        yield return new WaitForSeconds(t);
        Time.timeScale = 1;

    }



    public void EnemyKilled()
    {
        if (player != null)
        {
            player.GetComponent<WeaponHolder>().EnemyKilled();
        }
    }
}
