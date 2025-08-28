using UnityEngine;

[RequireComponent(typeof(EAI_StateMachine))]
[RequireComponent(typeof(EnemyContext))]
public class EnemyBootstrap : MonoBehaviour
{
    EAI_StateMachine fsm;
    EnemyContext ctx;

    IdleState idle;
    KeepRangeState keepRange;
    OrbitAttackState orbitAttack;

    void Awake()
    {
        fsm = GetComponent<EAI_StateMachine>();
        ctx = GetComponent<EnemyContext>();

        


    }

    void Start()
    {

    }

}

