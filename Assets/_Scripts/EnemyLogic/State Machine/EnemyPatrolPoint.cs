using UnityEngine;

public class EnemyPatrolPoint : MonoBehaviour
{
    public bool Claimed;
    public EnemyPatrolPoint nextInPatrol;
    public float WaitTime = 2;

    private void Start()
    {
        GameManager.instance.availablePatrolPoints.Add(this);
    }


    public void Claim()
    {
        Claimed = true;
        GameManager.instance.availablePatrolPoints.Remove(this);
    }

    public void MoveOn()
    {
        GameManager.instance.availablePatrolPoints.Add(this);
        Claimed = false;
    }


}
