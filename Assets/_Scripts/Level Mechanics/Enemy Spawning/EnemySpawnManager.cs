using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
struct PrefabSpawns
{
    [SerializeField] public Transform spawnTransform;
    [SerializeField] public GameObject enemyPrefab;
}
public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] List<GameObject> livingEnemies;
    [SerializeField] PrefabSpawns[] prefabSpawns;
    [SerializeField] bool spawnOnSceneLoad = false;
    [SerializeField] bool enemiesSpawned = false;
    [SerializeField] UnityEvent defeatEvent;

    void Start()
    {
        if (spawnOnSceneLoad) SpawnEnemies();
    }

    
    void Update()
    {
        if (!enemiesSpawned) return;

        //Remove dead enemies from list
        foreach (GameObject enemy in livingEnemies)
        {
            if (enemy == null) livingEnemies.Remove(enemy); 
        }

        if (livingEnemies.Count != 0) return;
        // Invoke defeat Events if any and destroy this object
        if (defeatEvent != null) defeatEvent.Invoke();
        Destroy(this.gameObject);
    }

    public void SpawnEnemies()
    {
        enemiesSpawned = true;
        livingEnemies.Clear();
        foreach(PrefabSpawns prefabSpawn in prefabSpawns)
        {
            livingEnemies.Add(GameObject.Instantiate(prefabSpawn.enemyPrefab, prefabSpawn.spawnTransform.position, prefabSpawn.spawnTransform.rotation));
        }
    }
}
