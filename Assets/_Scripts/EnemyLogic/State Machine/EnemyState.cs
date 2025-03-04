using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.OnScreen.OnScreenStick;

///<summary>
///The <c>EnemyState</c> class contains the list of behaviours belonging to an enemy state. 
///It organizes what variables need to be checked and when. 
///<para>It then calls upon the <c>ActionHandler.StartAction</c> method to execute the appropriate actions when their conditions are true.</para>
/// </summary>
[Serializable]
public class EnemyState
{
    public string Name;
    private EnemyBrain brain;
    protected Damagable damagable;
    protected EnemyStateMachine enemyStateMachine;


    public List<EnemyBehaviour> enemyBehaviours;
    private List<string> frameFunctionNames;
    private List<string> timeFunctionNames;
    private List<float> functionTimes;

    /// <summary>
    /// Instantiates <c>EnemyState</c> with the appropriate references and defines the list of functions for condition variable updating.
    /// </summary>
    /// <param name="damagable">Damagable instance on enemy object.</param>
    /// <param name="enemyStateMachine">EnemyStateMachine instance in EnemyBrain component.</param>
    /// <param name="enemyBehaviours">list of EnemyBehaviours defined in Unity Editor for this state.</param>
    /// <param name="Name">Name of this state.</param>
    /// <param name="brain">Reference to EnemyBrain component.</param>
    public EnemyState(Damagable damagable, EnemyStateMachine enemyStateMachine, List<EnemyBehaviour> enemyBehaviours, string Name, EnemyBrain brain)
    {
        this.damagable = damagable;
        this.enemyStateMachine = enemyStateMachine;
        this.enemyBehaviours = enemyBehaviours;
        this.brain = brain;
        this.Name = Name;
        InitializeConditions();
    }
    /// <summary>
    /// Loops over all behaviours in the state and their conditions to compile the list of function names to be called and at what intervals.
    /// <para>Defines the lists <c>frameFunctionNames</c>, <c>timeFunctionNames</c> and <c>functionTimes</c>. </para>
    /// </summary>
    private void InitializeConditions()
    {
        frameFunctionNames = new List<string>();
        timeFunctionNames = new List<string>();
        functionTimes = new List<float>();

        foreach (EnemyBehaviour behaviour in enemyBehaviours)
        {
            behaviour.InitializeBehaviour(brain);
            // setting which condition variables need to be updated every frame or every time interval
            behaviour.SetCheckTypes();
            

            // defining and concatenating lists of function names that need to be called to update condition variables
            frameFunctionNames.AddRange(behaviour.FrameChecks.boolCheck.Select(x => x.ReturnFunctionString()).ToList());
            frameFunctionNames.AddRange(behaviour.FrameChecks.floatCheck.Select(x => x.ReturnFunctionString()).ToList());
            frameFunctionNames.AddRange(behaviour.FrameChecks.intCheck.Select(x => x.ReturnFunctionString()).ToList());

            timeFunctionNames.AddRange(behaviour.TimeChecks.boolCheck.Select(x => x.ReturnFunctionString()).ToList());
            timeFunctionNames.AddRange(behaviour.TimeChecks.floatCheck.Select(x => x.ReturnFunctionString()).ToList());
            timeFunctionNames.AddRange(behaviour.TimeChecks.intCheck.Select(x => x.ReturnFunctionString()).ToList());
            

            functionTimes.AddRange(behaviour.TimeChecks.boolCheck.Select(x => x.checkTime).ToList());
            functionTimes.AddRange(behaviour.TimeChecks.floatCheck.Select(x => x.checkTime).ToList());
            functionTimes.AddRange(behaviour.TimeChecks.intCheck.Select(x => x.checkTime).ToList());

        }
        HandleFunctionDegeneracy(frameFunctionNames, timeFunctionNames, functionTimes);
    }

    /// <summary>
    /// Handles functions to be called when entering state, i.e. starting repeating condition variable updates.
    /// </summary>
    public virtual void EnterState() 
    {
        // Invoking repeating function calls every functionTimes seconds for updating condition variables
        brain.perception.StartRepeatingChecks(timeFunctionNames, functionTimes);
        brain.perception.UpdateFrameVariables(frameFunctionNames);
    }
    /// <summary>
    /// Handles functions to be called when exiting state: ceasing all repeating condition variable updates.
    /// </summary>
    public virtual void ExitState() 
    {
        // cancelling every repeating function call to prepare for next state
        brain.perception.StopRepeatingChecks();
    }


    int HighestPriorityIndex;
    int HighestPriority;

    /// <summary>
    /// Method that is called every frame in the enemy's <c>EnemyBrain</c> class instance.
    /// <para>
    /// Invokes condition variable updates, evaluates conditions and priority orders to execute the appropriate actions.
    /// </para>
    /// </summary>
    public virtual void FrameUpdate() 
    {
        HighestPriorityIndex = 0;
        HighestPriority = 0;
        int i = 0;
        //here: call functions that update condition variables every frame (wouldnt we do this in perception?) '' We are calling the perception UpdateFrameVariables method
        brain.perception.UpdateFrameVariables(frameFunctionNames);
        

        //checking whether current variable values satisfy behaviour conditions
        foreach (EnemyBehaviour behaviour in enemyBehaviours)
        {
            if (behaviour.conditions.CheckConditions())
            {
                if(behaviour.priority > HighestPriority)
                {
                    HighestPriority = behaviour.priority;
                    HighestPriorityIndex = i;
                }
            }
            i++;
        }

        //Priority Must Be set to larger than 0 for the Behaviour to be considered as valid (Allows for easier debuging for now)
        if(HighestPriority > 0)
        {
            DoAction(enemyBehaviours[HighestPriorityIndex]);
        }
    }

    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent() { }

    /// <summary>
    /// Executes actions in <paramref name="EB"/> either in order of list or randomly based on weights. (TO BE IMPLEMENTED)
    /// </summary>
    /// <param name="EB">instance of <c>EnemyBehaviour</c> whose actions are being executed.</param>
    public void DoAction(EnemyBehaviour EB)
    {
        //Consider Weights or do all actions in order
        Debug.Log("DoAction is called.");
        brain.actionHandler.StartAction(EB.actionList[0].Func());
    }

    ///<summary>
    ///This method removes the degeneracy in the lists of function names to be called to remove unnecessary function invocations. 
    ///<para>It does so by reassinging the EnemyState properties of these lists to the non-degenerate result.</para>  
    ///</summary>
    ///<param name="frameFunctions">list of function names that are called every frame</param>
    ///<param name="timeFunctions">list of function names that are called every t seconds</param>
    ///<param name="times">list of times for repeating function invokes</param>
    private void HandleFunctionDegeneracy(List<string> frameFunctions, List<string> timeFunctions, List<float> times)
    {
        List<string> distinctFrameFunctions = new List<string>();
        List<string> distinctTimeFunctions = new List<string>();
        List<float> distinctTimes = new List<float>();
        distinctFrameFunctions = frameFunctions.Distinct().ToList();
        for (int i = 0; i < timeFunctions.Count; i++)
        {
            if (!distinctFrameFunctions.Contains(timeFunctions[i]))
            {
                if (!distinctTimeFunctions.Contains(timeFunctions[i]))
                {
                    distinctTimeFunctions.Add(timeFunctions[i]); distinctTimes.Add(times[i]);
                }
                else
                {
                    int index = distinctTimeFunctions.IndexOf(timeFunctions[i]);
                    if (times[i] < distinctTimes[index]) distinctTimes[index] = times[i];
                }
            }
        }
        frameFunctionNames = distinctFrameFunctions;
        timeFunctionNames = distinctTimeFunctions;
        functionTimes = distinctTimes;
    }
}
