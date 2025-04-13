using UnityEngine;

public class Weapon_Attack_Data_Raycast : Weapon_Attack_Data_Base
{
    public float Range;

    // Constructor for Weapon_Attack_Melee
    public Weapon_Attack_Data_Raycast()
    {
        attackType = AttackType.Raycast;
    }



    public override void PerformAttack(WeaponHolder W)
    {
        base.PerformAttack(W);
        Debug.Log("Did the thing");
        RaycastHit hit;
       
        if (Physics.Raycast(W.cam.transform.position, W.cam.transform.forward, out hit, Range))
        {
            ApplyKnockback(hit.collider, W.cam.transform.forward);
            if (hit.collider.gameObject.layer == 8)
            {
                Debug.Log("Raycast hit!");
                hit.collider.gameObject.GetComponent<Damagable>().TakeDamage(damage);
            }
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
