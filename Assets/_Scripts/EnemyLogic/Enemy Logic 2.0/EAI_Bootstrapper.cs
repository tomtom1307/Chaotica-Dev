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

        // Allocate states once
        idle = new IdleState(ctx);
        keepRange = new KeepRangeState(ctx);
        orbitAttack = new OrbitAttackState(ctx);

        //TODO place replace next function to use made states as opposed to creating a new state each time.



    }

    void Start() => fsm.Set(idle);

}

