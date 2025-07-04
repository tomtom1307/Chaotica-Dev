using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityHUDController : MonoBehaviour
{
    public HUDAbility AbilityHudPrefab;
    List<Ability> abilities = new List<Ability>();
    List<HUDAbility> HUDAbilities = new List<HUDAbility>();
    private void Start()
    {
        GetAbilities();
    }


    public void GetAbilities()
    {
        List<AbilityHolder> abilityHolders = PlayerStats.instance.GetComponents<AbilityHolder>().ToList();
        foreach (var abilityHolder in abilityHolders)
        {
            if (abilityHolder.ability != null) 
            {
                abilities.Add(abilityHolder.ability);
                HUDAbility hUDAbility = Instantiate(AbilityHudPrefab, this.transform);
                hUDAbility.SetAbility(abilityHolder);
                HUDAbilities.Add(hUDAbility);
                
            }

            
        }

    }
}
