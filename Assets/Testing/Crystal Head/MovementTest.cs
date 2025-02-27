using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MovementTest : MonoBehaviour
{
    public List<Transform> PatrolPoints;
    public float Waittime = 5;
    Animator anim;
    int maxCounter;
    public int counter = 0;
    private float distancetoNextPoint;
    public float matchingfactor;
    NavMeshAgent agent;
    private bool chilling;
    new AudioSource audio;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        maxCounter = PatrolPoints.Count;
        Chill();
    }

    private void Update()
    {
        if (!chilling && agent.remainingDistance == 0) Chill();
    }


    public void ChangePosition(Transform newPos)
    {
        agent.destination = PatrolPoints[counter].position;
        audio.Play();
        anim.SetFloat("WalkSpeed", agent.speed * matchingfactor);
        anim.SetBool("Walking", true);
    }

    public void MoveAnim()
    {
        chilling = false;
        ChangePosition(PatrolPoints[counter]);
    }

    public void Chill()
    {
        audio.Stop();
        chilling = true;
        counter++;
        if (counter == maxCounter)
        {
            counter = 0;
        }
        anim.SetBool("Walking", false);
        Invoke("MoveAnim", Waittime);
    }
}
