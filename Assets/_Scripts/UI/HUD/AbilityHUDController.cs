using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityHUDController : MonoBehaviour
{
    public HUDAbility AbilityHudPrefab;
    List<Ability> abilities;
    List<HUDAbility> HUDAbilities;
    private void Start()
    {
        GetAbilities();
    }


    public void GetAbilities()
    {
        List<AbilityHolder> abilityHolders = PlayerStats.instance.GetComponents<AbilityHolder>().ToList();
        foreach (var abilityHolder in abilityHolders)
        {
            abilities.Add(abilityHolder.ability);

            HUDAbility hUDAbility = Instantiate(AbilityHudPrefab);
            hUDAbility.SetAbility(abilityHolder.ability);
            HUDAbilities.Add(hUDAbility);
        }

    }
}
