using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    float cooldownTime;
    float activeTime;
    HUDAbility hUD;
    
    enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    AbilityState abilityState = AbilityState.ready;

    public KeyCode key;

    void SwapAbility()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        if (ability == null)
        {
            return;
        }

        switch (abilityState)
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    // activate
                    ability.Activate(gameObject);
                    abilityState = AbilityState.active;
                    activeTime = ability.activeTime;
                    
                }
            break;
            case AbilityState.active:
                //time that ability is used
                if (activeTime > 0)
                {
                    hUD.AbilityActive();
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    abilityState = AbilityState.cooldown;
                    cooldownTime = ability.cooldownTime;
                }
            break;
            case AbilityState.cooldown:
                //time that ability cant be used
                if (cooldownTime > 0)
                {
                    hUD.AbilityCooldown(cooldownTime);
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    hUD.AbilityReady();
                    abilityState = AbilityState.ready;
                }
            break;
        }
        
    }

    public void SetHUD(HUDAbility HUD)
    {
        hUD = HUD;
    }
}
