using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        
    }
}
