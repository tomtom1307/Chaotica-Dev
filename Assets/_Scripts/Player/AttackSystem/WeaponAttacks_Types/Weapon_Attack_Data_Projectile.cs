using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Weapon_Attack_Data_Projectile : Weapon_Attack_Data_Base
{

    public GameObject projectile;
    public int Projectile_Amount = 1;
    public float spread = 5;
    public float speed = 15;
    public bool gravity;

    // Constructor for Weapon_Attack_Melee
    public Weapon_Attack_Data_Projectile()
    {
        attackType = AttackType.Projectile;
    }

    

    public override void PerformAttack(WeaponHolder W)
    {
        base.PerformAttack(W);
        Debug.Log("Did the thing");
        
        for (int i = 0; i < Projectile_Amount; i++)
        {

            Vector3 dir = W.cam.transform.forward;
            dir += W.cam.transform.up * UnityEngine.Random.Range(-spread, spread);
            dir += W.cam.transform.right * UnityEngine.Random.Range(-spread, spread);
            dir.Normalize(); 

            GameObject proj = W.SpawnObject(projectile, W.cam.transform.position, Quaternion.identity);
            proj.GetComponent<Projectile>().Init(dir, speed, DamageVal(W), gravity);
        }

    }


    public override void EnterAttack(WeaponHolder W)
    {
        base.EnterAttack(W);
    }


    public override void ExitAttack(WeaponHolder W)
    {
        base.ExitAttack(W);
    }

}
