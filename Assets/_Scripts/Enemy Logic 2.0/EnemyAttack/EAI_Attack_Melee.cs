using UnityEngine;

[CreateAssetMenu(fileName = "EAI_Attack_Melee", menuName = "AI/Attacks/Melee")]
public class EAI_Attack_Melee : EAI_AttackSO_Base
{


    public override void Enter(EnemyContext EC)
    {
        base.Enter(EC);
    }

    public override void Execute(EnemyContext EC)
    {
        base.Execute(EC);
        //Is handled through collider group logic in animation events
    }

    public override void Exit(EnemyContext EC)
    {
        base.Exit(EC);
    }
}
