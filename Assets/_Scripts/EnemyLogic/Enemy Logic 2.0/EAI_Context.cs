using UnityEditor.Experimental.GraphView;
using UnityEngine;


//Holds all references
public class EnemyContext : MonoBehaviour
{
    public EnemyTuningSO cfg;
    public Transform muzzle;
    public bool DebugStates;
    [HideInInspector] public Blackboard bb;
    [HideInInspector] public IMotor motor;
    [HideInInspector] public Aimer aimer;
    [HideInInspector] public IAbilityRunner abilities;
    [HideInInspector] public IPerception sense;
    [HideInInspector] public float nextBurstAt;

    void Awake()
    {
        bb = GetComponent<Blackboard>();
        motor = GetComponent<IMotor>();
        aimer = GetComponent<Aimer>();
        abilities = GetComponent<IAbilityRunner>();
        sense = GetComponent<IPerception>();
    }
}
