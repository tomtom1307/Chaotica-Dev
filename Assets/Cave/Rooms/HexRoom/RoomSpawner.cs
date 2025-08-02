using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public float SpawnRange;
    public int currentRound;
    public List<SpawnerRound> Rounds;
    RoomManager roomManager;
    bool Started = false;
    public List<EnemyBrain> spawnedEnemies;
    public int EnemiesLeft;

    private void Start()
    {
        roomManager = GetComponent<RoomManager>();
    }

    private void Update()
    {
        if(EnemiesLeft <= 0 && Started)
        {
            RoundComplete();
        }
    }

    public void TriggerSpawner()
    {
        TriggerRound(0);
        Started = true;
    }


    public void TriggerRound(int i)
    {
        for (int j = 0; j < Rounds[i].EnemyAmount; j++)
        {
            var Enemy = SampleEnemyDistribution();
            SpawnEnemy(Enemy);
        }
    }

    public GameObject SampleEnemyDistribution()
    {
        EnemyDistributionBase Distribution = GameManager.instance.enemyDistributions[GameManager.instance.EnemyDifficultyLevel];

        //Calculate Total Prob
        float Total = 0;
        foreach (var Entry in Distribution.EnemyDistribution)
        {
            Total += Entry.prob;
        }

        float rand = UnityEngine.Random.Range(0f, Total);
        float cumulative = 0f;

        foreach (var entry in Distribution.EnemyDistribution)
        {
            cumulative += entry.prob;
            if (rand <= cumulative)
            {
                return entry.EnemyPrefab;
            }
        }
        return null;

    }

    public void SpawnEnemy(GameObject Prefab)
    {
        Vector3 pos = transform.position;
        Vector3 Rand = new Vector3(UnityEngine.Random.Range(-SpawnRange, SpawnRange), 0, UnityEngine.Random.Range(-SpawnRange, SpawnRange));
        
        GameObject Enemy = Instantiate(Prefab, pos + Rand, Quaternion.identity);
        Enemy.GetComponent<EnemyBrain>().SetSpawner(this);
        EnemiesLeft++;

    }

    public void RoundComplete()
    {
        currentRound++;
        if(currentRound > Rounds.Count)
        {
            SpawnerCompleted();
        }
        else
        {
            TriggerRound(currentRound);
        }
    }

    public void SpawnerCompleted()
    {
        roomManager.RoomComplete();
    }

}

[Serializable]
public class SpawnerRound 
{
    public int EnemyAmount;
    
    //TODO : Override Distribution
}




