using UnityEngine;

[CreateAssetMenu(fileName = "EAI_Attack_Melee", menuName = "AI/Attacks/Melee")]
public class EAI_Attack_Melee : EAI_AttackSO_Base
{
    public override void Enter(GameObject owner, in AbilityContext ctx)
    {
        base.Enter(owner, ctx);
    }

    public override void Execute(GameObject owner, in AbilityContext ctx)
    {
        base.Execute(owner, ctx);
    }

    public override void Exit(GameObject owner, in AbilityContext ctx)
    {
        base.Exit(owner, ctx);
    }
}
