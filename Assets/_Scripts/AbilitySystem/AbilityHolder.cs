using System;
using System.Collections;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    float cooldownTime;
    float activeTime;
    HUDAbility hUD;

    public bool IsAbilityActive => abilityState == AbilityState.active;

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
                    if(!ability.Check(gameObject, this))
                    {
                        return;
                    }
                    abilityState = AbilityState.active;
                    ability.Activate(gameObject, this);
                    activeTime = ability.activeTime;
                    
                }
            break;
            case AbilityState.active:
                //time that ability is used
                if (activeTime > 0)
                {
                    ability.AbilityUpdate(gameObject, this);
                    hUD.AbilityActive();
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    abilityState = AbilityState.cooldown;
                    ability.Deactivate(gameObject, this);
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


    #region Chain Lightning

    public void StartCoroutineUpdateLineRenderer(GameObject lineR, Transform startPos, Transform endPos, bool fromPlayer = false)
    {
        StartCoroutine(UpdateLineRenderer(lineR, startPos, endPos, fromPlayer));
    }

    float RefreshRate = 0.01f;

    IEnumerator UpdateLineRenderer(GameObject lineR, Transform startPos, Transform endPos, bool fromPlayer = false)
    {
        ChainLightning CL_ability = ability as ChainLightning;
        if (abilityState  == AbilityState.active)
        {
            Debug.Log("Updated LR");
            lineR.GetComponent<LightningLR>().SetPosition(startPos, endPos);
            RefreshRate = 0.01f;
            yield return new WaitForSeconds(RefreshRate);

            if (fromPlayer)
            {
                
                StartCoroutine(UpdateLineRenderer(lineR, startPos, ability.GetBestEnemy(CL_ability.JumpRange, CL_ability.MaxViewAngle, gameObject).transform));
            }
            else
            {
                StartCoroutine(UpdateLineRenderer(lineR, startPos, endPos));
            }

            
        }
    }
    #endregion
}
