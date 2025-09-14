using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityRunner : MonoBehaviour, IAbilityRunner
{
    Dictionary<string, float> nextReady = new();

    public bool CanUse(EAI_AbilitySO a) => a && Time.time >= (nextReady.TryGetValue(a.abilityId, out var t) ? t : 0f);

    public void Use(EAI_AbilitySO a, in AbilityContext ctx)
    {
        if (!CanUse(a)) return;
        a.Execute(gameObject, ctx);
        nextReady[a.abilityId] = Time.time + a.cooldown;
    }
}
