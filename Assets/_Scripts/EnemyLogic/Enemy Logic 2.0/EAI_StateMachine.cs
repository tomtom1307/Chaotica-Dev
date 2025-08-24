using System;
using Unity.Collections;
using UnityEngine;


public interface IState
{
    string Name { get; }
    void OnEnter();
    void Tick();
    void OnExit();
    IState Next();  // return new state to transition, or null to stay
}

public class EAI_StateMachine : MonoBehaviour
{
    public IState Current;

    [SerializeField] string currentStateName;

    public void Set(IState s)
    {
        if (Current == s) return;

        Current?.OnExit();
        Current = s;
        Current?.OnEnter();

        // Update debug field
        currentStateName = Current?.Name;
    }

    void Update() 
    {
        currentStateName = Current.GetType().Name;
        Current?.Tick(); 
        var n = Current?.Next(); 
        if (n != null) Set(n);
    }
}


public abstract class BaseState : IState
{

    

    protected Func<IState> nextFn;

    public string Name => null;

    public void SetTransitions(Func<IState> next) => nextFn = next;
    public virtual void OnEnter() { }
    public virtual void Tick() { }
    public virtual void OnExit() { }
    public virtual IState Next() => nextFn?.Invoke();
}



public static class SteeringHelpers
{
    public static Vector3 Chase(Transform self, Transform target, float speed)
    {
        if (!target) return Vector3.zero;
        var to = (target.position - self.position).normalized * speed;
        return new Vector3(to.x, 0f, to.z);
    }

    public static Vector3 Flee(Transform self, Transform target, float speed)
    {
        if (!target) return Vector3.zero;
        var away = (self.position - target.position).normalized * speed;
        return new Vector3(away.x, 0f, away.z);
    }

    public static Vector3 KeepRange(Transform self, Transform target, float speed, float min, float max)
    {
        if (!target) return Vector3.zero;
        var to = target.position - self.position;
        float d = to.magnitude;
        if (d < 0.001f) return Vector3.zero;
        if (d < min) return (-to.normalized) * speed;  // back off
        if (d > max) return (to.normalized) * speed;  // close in
        return Vector3.zero;                             // already in band
    }
}
