using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TRTools.floatOP;
using static TRTools.Weighted_Distribution;

public class EnemyAttackState : BaseState
{
    public EAI_AttackSO_Base _currentAttack;
    public AbilityContext _currentAbilityCtx;

    public EnemyAttackState(EnemyContext ctx) : base(ctx)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        c.bb.attack_State = EAI_Blackboard.AttackState.attacking;
        AbilityEntry abilityEntry = DetermineAttack();
        _currentAttack = abilityEntry.Ability as EAI_AttackSO_Base;
        if( _currentAttack == null ) { c.bb.attack_State = EAI_Blackboard.AttackState.ready; c.attackHandler.AttackCooldown(0.5f); }
        
        //Handles animation and detection priming
        c.attackHandler.HandleAttackEnter( abilityEntry );
        c.aimer.AimAt(c.bb.target.position);
        _currentAttack.Enter(c);
        
    }

    public override void Tick()
    {
     
        base.Tick();
        if (_currentAttack != null)
        {
            _currentAttack.Tick(c);
        }
    }

    public override void OnExit()
    {
        c.anim.ApplyRootMotion(false);
        base.OnExit();
    }

    public AbilityEntry DetermineAttack()
    {
        AbilityEntry Default = new AbilityEntry(); //Gives empty AbilityEntry
        Default.Ability = null;

        List<AbilityEntry> attacks = new List<AbilityEntry>(c.cfg.Attacks);
        List<AbilityEntry> ValidAttacks = new List<AbilityEntry>();
        if (attacks.Count > 0)
        {
            foreach (var attack in attacks)
            {
                if (c.attackHandler.CanDoAttack(attack))
                {
                    ValidAttacks.Add(attack);
                }

            }
            if(ValidAttacks.Count > 0) {
                var Chosen = Sample_Weighted_Distribution<AbilityEntry>(ValidAttacks, e => e.Weight);
                return Chosen;
            }
            else
            {
                Debug.LogError("Enemy tried to attack but has no Attacks");
                return Default; //Gives empty AbilityEntry
            }
        }
        else
        {
            Debug.LogError("Enemy tried to attack but has no Attacks");
            return Default;
        }
    }




}
