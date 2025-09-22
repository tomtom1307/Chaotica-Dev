using UnityEngine;

[CreateAssetMenu(fileName = "EAI_Attack_Raycast", menuName = "AI/Attacks/Raycast")]
public class EAI_Attack_Raycast : EAI_AttackSO_Base
{
    public float AttackRange;
    public override void Enter(EnemyContext EC, in AbilityContext ctx)
    {
        base.Enter(EC, ctx);
    }

    public override void Execute(EnemyContext EC, in AbilityContext ctx)
    {
        base.Execute(EC, ctx);
        EC.attackHandler.DoRayCast();
    }

    public override void Exit(EnemyContext EC, in AbilityContext ctx)
    {
        base.Exit(EC, ctx);
    }

    public override void Tick(EnemyContext EC, in AbilityContext A_ctx)
    {
        base.Tick(EC, A_ctx);
        EC.attackHandler.AimDirection = EC.LookDirection.forward;
    }
}
