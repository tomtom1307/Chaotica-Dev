using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnList SO;
    public float Range;

    public int AttemptLimit;
    public virtual void Trigger()
    {
        foreach (var spawn in SO.spawnList) 
        {
            for (int j = 0; j < spawn.amount; j++)
            {
                Vector3 pos = GenerateRandomPositionInCircle(Range);
                int i = 0;
                while (!isValidSpawn(pos))
                {
                    pos = GenerateRandomPositionInCircle(Range);
                    if (i >= AttemptLimit)
                    {
                        break;
                    }
                    i++;
                }

                SpawnEnemy(spawn.Enemy, pos);
            }

        }
    }

    public Vector3 GenerateRandomPositionInCircle(float X)
    {
        float x = UnityEngine.Random.Range(-Range, Range);
        float z = UnityEngine.Random.Range(-Range, Range);

        return transform.position + new Vector3(x, 0, z);
    }

    public virtual void SpawnEnemy(GameObject GO,Vector3 Position)
    {
        var enemy = Instantiate(GO, Position, Quaternion.identity);
        Animator anim = enemy.GetComponent<Animator>();
        anim.Play("Spawn");
        anim.applyRootMotion = false;

    }

    public virtual bool isValidSpawn(Vector3 pos)
    {
        //DoRayCast
        return true;
    }


}


