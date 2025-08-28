using UnityEngine;
using static WeaponHolder;

public sealed class Standard_ESI : EnemyStateInstaller
{
    public override void Install(EAI_StateMachine sm, EnemyContext ctx)
    {
        var idle = sm.Register(new IdleState(ctx));
        var patrol = sm.Register(new PatrolState(ctx));
        var chase = sm.Register(new KeepRangeState(ctx));
        var orbit = sm.Register(new OrbitAttackState(ctx));
        var attack = sm.Register(new EnemyAttackState(ctx));
        var search = sm.Register(new SearchState(ctx));

        // Local transitions (declare inside the states)
        idle.AddTransition(() => ctx.bb.DetectionMeter == 1f, chase);
        idle.AddTransition(() => ctx.bb.DetectionMeter >= 0.4f, search);

        patrol.AddTransition(() => ctx.bb.DetectionMeter >= 1f, chase);

        chase.AddTransition(() => ctx.bb.isInRange, orbit);
        chase.AddTransition(() => !ctx.bb.hasLOS && ctx.bb.LastSeenPlayerTime >= ctx.cfg.GoBackToSearchTime, search);

        search.AddTransition(() => ctx.bb.DetectionMeter >= 1f, chase);
        search.AddTransition(() => ctx.bb.LastSeenPlayerTime >= ctx.cfg.SearchDuration, idle);

        orbit.AddTransition(() => !ctx.bb.hasLOS && ctx.bb.LastSeenPlayerTime >= ctx.cfg.GoBackToSearchTime, search);
        orbit.AddTransition(() => ctx.bb.isInRange && ctx.bb.ReadyToAttack, attack);
        orbit.AddTransition(() => !ctx.bb.isInRange, chase);


        sm.Set(idle);

        sm.AddAnyTransition<SearchState>(() => ctx.bb.InvestigateSound && !ctx.bb.hasLOS);

    }
}
