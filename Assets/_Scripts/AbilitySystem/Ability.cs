using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.WSA;


public class Ability : ScriptableObject
{
    public Sprite Display;
    public new string name;
    public float activeTime;
    public float cooldownTime;
    [SerializeField] protected LayerMask EnemyLayerMask;
    protected AbilityHolder holder;

    public virtual bool Check(GameObject parent, AbilityHolder holder)
    {
        return true;
    }

    public virtual void Activate(GameObject parent, AbilityHolder holder)
    {
        this.holder = holder;
        //yasbbab
    }


    public virtual void AbilityUpdate(GameObject parent, AbilityHolder holder)
    {

    }

    public virtual void Deactivate(GameObject parent, AbilityHolder holder)
    {

    }

    public GameObject GetClosestEnemy(float Radius, GameObject parent)
    {
        List<Collider> enemiesInRange = new List<Collider>();
        enemiesInRange = Physics.OverlapSphere(parent.transform.position, Radius, EnemyLayerMask).ToList();

        if (enemiesInRange.Count > 0)
        {
            GameObject bestTarget = null;
            float closestDistance = Mathf.Infinity;
            Vector3 currentPosition = parent.transform.position;

            foreach (Collider enemy in enemiesInRange)
            {
                Vector3 directionToTarget = enemy.transform.position - currentPosition;
                float distToTarget = directionToTarget.sqrMagnitude;

                if (distToTarget < closestDistance)
                {
                    closestDistance = distToTarget;
                    bestTarget = enemy.gameObject;
                }
            }
            return bestTarget;
        }
        else
        {
            return null;
        }

    }


    //Returns Enemy that player is looking at the most within a radius
    public GameObject GetBestEnemy(float Radius, float MaxAngle,GameObject parent)
    {
        GameObject bestTarget = null;
        float closestAngle = Mathf.Infinity;

        List<Collider> Colliders = Physics.OverlapSphere(parent.transform.position, Radius, EnemyLayerMask).ToList();

        foreach (Collider col in Colliders)
        {
            Vector3 dirToEnemy = (col.transform.position - Camera.main.transform.position).normalized;
            float angle = Vector3.Angle(Camera.main.transform.forward, dirToEnemy);

            if (angle < MaxAngle && angle < closestAngle)
            {
                closestAngle = angle;
                if(col != null) bestTarget = col.gameObject;

            }
        }
        return bestTarget;
    }

}
