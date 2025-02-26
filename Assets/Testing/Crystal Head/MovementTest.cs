using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementTest : MonoBehaviour
{
    public List<Transform> PatrolPoints;
    public float Waittime = 5;
    Animator anim;
    public float Runspeed;
    int maxCounter;
    public int counter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        maxCounter = PatrolPoints.Count;
        Chill();
    }

    private void Update()
    {
        if (counter == maxCounter)
        {
            counter = 0;
        }

    }


    public void ChangePosition(Transform newPos)
    {
        transform.DOLookAt(newPos.position, 0.5f);
        anim.SetBool("Walking", true);
        transform.DOMove(newPos.position, Runspeed).OnComplete(Chill).SetEase(Ease.Linear);

    }

    public void MoveAnim()
    {
        ChangePosition(PatrolPoints[counter]);
    }

    public void Chill()
    {
        transform.DORotate((PatrolPoints[counter].rotation.eulerAngles), 0.8f);
        anim.SetBool("Walking", false);
        counter++;
        Invoke("MoveAnim", Waittime);
    }
}
