using System.Collections.Generic;
using UnityEngine;
using static TRTools.floatOP;

public class EnemyAbilityRunner : MonoBehaviour
{

    public enum AbilityState
    {
        Ready,
        Active,
        Cooldown
    }

    public AbilityState State;
    EAI_AbilitySO current_ability;
    AbilityContext CurrentAbilityCtx;

    EnemyContext EC;
    private void Start()
    {
        EC = GetComponent<EnemyContext>();
    }

    public bool CanUse(AbilityEntry a)
    {
        bool _inRange = InRange(EC.bb.distanceToTarget, a.MinRange, a.MaxRange);
        bool _enabled = a.Enabled;
        return _inRange && _enabled;
    }

    float timer;

    public void Use(AbilityEntry a, in AbilityContext A_ctx)
    {
        
    }

    private void Update()
    {
        if(current_ability != null && State == AbilityState.Active) { current_ability.Tick(EC, CurrentAbilityCtx); }


    }

}
