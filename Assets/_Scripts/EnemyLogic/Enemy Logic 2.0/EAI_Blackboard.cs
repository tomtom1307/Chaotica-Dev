using UnityEngine;


//Holds all Relevant information from perception
public class Blackboard : MonoBehaviour
{
    public Transform target;
    public float DetectionMeter;
    public Vector3 lastKnownTargetPos;
    public float distanceToTarget;
    public bool hasLOS;
}

