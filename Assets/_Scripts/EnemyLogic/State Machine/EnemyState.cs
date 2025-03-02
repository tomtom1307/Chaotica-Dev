using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.OnScreen.OnScreenStick;

public class EnemyState     
{
    private EnemyBrain brain;
    protected Damagable damagable;
    protected EnemyStateMachine enemyStateMachine;


    protected List<EnemyBehaviour> enemyBehaviours;
    private List<string> frameFunctionNames;
    private List<string> timeFunctionNames;
    private List<float> functionTimes;


    public EnemyState(Damagable damagable, EnemyStateMachine enemyStateMachine, List<EnemyBehaviour> enemyBehaviours, EnemyBrain brain)
    {
        this.damagable = damagable;
        this.enemyStateMachine = enemyStateMachine;
        this.enemyBehaviours = enemyBehaviours;
        this.brain = brain;
        InitializeConditions();
    }
    private void InitializeConditions()
    {
        

        foreach (EnemyBehaviour behaviour in enemyBehaviours)
        {
            behaviour.InitializeBehaviour(brain);
            // setting which condition variables need to be updated every frame or every time interval
            behaviour.SetCheckTypes();

            // defining and concatenating lists of function names that need to be called to update condition variables
            frameFunctionNames = behaviour.FrameChecks.boolCheck.Select(x => x.ReturnFunctionString()).ToList();
            frameFunctionNames.AddRange(behaviour.FrameChecks.floatCheck.Select(x => x.ReturnFunctionString()).ToList());
            frameFunctionNames.AddRange(behaviour.FrameChecks.intCheck.Select(x => x.ReturnFunctionString()).ToList());

            timeFunctionNames = behaviour.TimeChecks.boolCheck.Select(x => x.ReturnFunctionString()).ToList();
            timeFunctionNames.AddRange(behaviour.TimeChecks.floatCheck.Select(x => x.ReturnFunctionString()).ToList());
            timeFunctionNames.AddRange(behaviour.TimeChecks.intCheck.Select(x => x.ReturnFunctionString()).ToList());
            

            functionTimes = behaviour.TimeChecks.boolCheck.Select(x => x.checkTime).ToList();
            functionTimes.AddRange(behaviour.TimeChecks.floatCheck.Select(x => x.checkTime).ToList());
            functionTimes.AddRange(behaviour.TimeChecks.intCheck.Select(x => x.checkTime).ToList());

        }
        
    }


    public virtual void EnterState() 
    {
        foreach (EnemyBehaviour behaviour in enemyBehaviours)
        {
            // Invoking repeating function calls every functionTimes seconds for updating condition variables
            brain.perception.StartRepeatingChecks(timeFunctionNames, functionTimes); // TODO: REMOVE DUPLICATE FUNCTION CALLS FROM DIFFERENT BEHAVIOURS AND STATE TRANSITIONS in HandleFunctionDegeneracy()
        }
    }
    public virtual void ExitState() 
    {
        foreach (EnemyBehaviour behaviour in enemyBehaviours)
        {
            // cancelling every repeating function call to prepare for next state
            brain.perception.StopRepeatingChecks();
        }
    }
    public virtual void FrameUpdate() 
    {
        //here: call functions that update condition variables every frame (wouldnt we do this in perception?)


        //checking whether current variable values satisfy behaviour conditions
        foreach (EnemyBehaviour behaviour in enemyBehaviours)
        {
            Debug.Log(frameFunctionNames.Count);
            brain.perception.UpdateFrameVariables(frameFunctionNames);
            if (behaviour.conditions.CheckConditions())
            {
                Debug.Log(true);
                brain.actionHandler.StartAction(behaviour.actionList[0].Func());
            }
            else
            {
                Debug.Log(false);
            }
        }
    }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent() { }

    private void HandleFunctionDegeneracy()
    {

    }
}
