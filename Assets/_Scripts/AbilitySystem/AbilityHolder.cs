using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    float cooldownTime;
    float activeTime;

    
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
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    abilityState = AbilityState.ready;
                }
            break;
        }
        
    }
}
