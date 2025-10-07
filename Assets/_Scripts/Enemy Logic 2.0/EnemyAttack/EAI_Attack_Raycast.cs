using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "EAI_Attack_Raycast", menuName = "AI/Attacks/Raycast")]
public class EAI_Attack_Raycast : EAI_AttackSO_Base
{
    public LayerMask RaycastLayers;
    public float AttackRange;
    public override void Enter(EnemyContext EC)
    {
        base.Enter(EC);
        EC.attackHandler.SetIndicatorGlow(0);
        EC.attackHandler.AimDirection = EC.LookDirection.forward;
        Physics.Raycast(EC.transform.position, EC.attackHandler.AimDirection, out RaycastHit hit, AttackRange);
        EC.attackHandler.UpdateIndicator(hit.point);
        EC.attackHandler.IndicatorActive(true);
    }

    public override void Execute(EnemyContext EC)
    {
        base.Execute(EC);
        EC.attackHandler.IndicatorActive(false);
        EC.attackHandler.DoRayCast();
    }

    public override void Exit(EnemyContext EC)
    {
        
        base.Exit(EC);
    }

    public override void Tick(EnemyContext EC)
    {
        base.Tick(EC);
        EC.attackHandler.AimDirection = EC.LookDirection.forward;

        //Check if laser hit
        Physics.Raycast(EC.transform.position, EC.attackHandler.AimDirection, out RaycastHit hit, AttackRange);
        EC.attackHandler.CurrentlyTargetedPos = hit.point;
    }

    public override void LateTick(EnemyContext EC)
    {
        base.LateTick(EC);

        //UpdateLaser
        EC.attackHandler.UpdateIndicator(EC.attackHandler.CurrentlyTargetedPos);
    }

    
}
