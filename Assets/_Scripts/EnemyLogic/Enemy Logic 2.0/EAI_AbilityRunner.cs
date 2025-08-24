using System.Collections.Generic;
using UnityEngine;

public class AbilityRunner : MonoBehaviour, IAbilityRunner
{
    Dictionary<string, float> nextReady = new();

    public bool CanUse(AbilitySO a) => a && Time.time >= (nextReady.TryGetValue(a.abilityId, out var t) ? t : 0f);

    public void Use(AbilitySO a, in AbilityContext ctx)
    {
        if (!CanUse(a)) return;
        a.Execute(gameObject, ctx);
        nextReady[a.abilityId] = Time.time + a.cooldown;
    }
}
