using UnityEngine;

[CreateAssetMenu(fileName = "EAI_Attack_Melee", menuName = "AI/Attacks/Melee")]
public class EAI_Attack_Melee : EAI_AttackSO_Base
{


    public override void Enter(EnemyContext EC, in AbilityContext ctx)
    {
        base.Enter(EC, ctx);
    }

    public override void Execute(EnemyContext EC, in AbilityContext ctx)
    {
        base.Execute(EC, ctx);
        //Is handled through collider group logic in animation events
    }

    public override void Exit(EnemyContext EC, in AbilityContext ctx)
    {
        base.Exit(EC, ctx);
    }
}
