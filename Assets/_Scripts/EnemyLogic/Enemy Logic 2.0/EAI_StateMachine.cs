using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public interface IState
{
    string Name { get; }
    void OnEnter();
    void Tick();
    void OnExit();
    public IState Next();  // return new state to transition, or null to stay
}

public abstract class BaseState : IState
{
    protected readonly EnemyContext c;

    protected BaseState(EnemyContext ctx) => this.c = ctx;

    public virtual string Name => GetType().Name;

    protected Func<IState> nextFn;
    public virtual void OnEnter() { }
    public virtual void Tick() { }
    public virtual void OnExit() { }

    public void AddTransition(Func<bool> when, IState to)
    {
        var Test = (when, to);
        Debug.Log(Test);
        _transitions.Add((when, to));
        Debug.Log("Transition Added" + to.Name + _transitions.Count);
        
    }

    private List<(Func<bool> when, IState to)> _transitions = new();

    public virtual IState Next()
    {
        for (int i = 0; i < _transitions.Count; i++)
        {
            var (when, to) = _transitions[i];
            if (when()) return to;
        }
        return null; // stay in state
    }
}


// Optional global-transition
internal sealed class Transition
{
    public readonly Func<bool> When;
    public readonly IState To;
    public Transition(Func<bool> when, IState to) { When = when; To = to; }
}


public class EAI_StateMachine : MonoBehaviour
{
    public IState Current { get; private set; }
    [SerializeField] string currentStateName;

    private EnemyContext ctx;

    private readonly Dictionary<Type, IState> _states = new();
    private readonly List<Transition> _anyTransitions = new();


    private void Awake()
    {
        ctx = GetComponent<EnemyContext>();

        var installer = GetComponent<EnemyStateInstaller>();
        if (!installer)
        {
            Debug.LogError("Missing EnemyStateInstaller on this enemy.");
            enabled = false;
            return;
        }


        installer.Install(this, ctx);

        if (Current == null)
        {
            Debug.LogError("No initial state set in installer.");
            enabled = false;
        }
        else
        {
            currentStateName = Current.Name;
        }
    }

    private void Start()
    {
        
    }

    public void Set(IState s)
    {
        if (s == null || s == Current) return;
        Current?.OnExit();
        Current = s;
        Current.OnEnter();
    }

    private void Update()
    {
        // Global (Any) transitions first
        var any = GetTriggeredAny();
        if (any != null && any != Current)
        {
            Set(any);
        }
        else
        {
            // Normal flow
            Current?.Tick();
            var next = Current?.Next();
            if (next != null && next != Current) Set(next);
        }

        currentStateName = Current?.Name;
        if (ctx && ctx.bb != null) ctx.bb.CurrentState = currentStateName;
    }

    private IState GetTriggeredAny()
    {
        for (int i = 0; i < _anyTransitions.Count; i++)
        {
            var t = _anyTransitions[i];
            if (t.When()) return t.To;
        }
        return null;
    }

    public T Register<T>(T state) where T : IState
    {
        _states[typeof(T)] = state;
        return state;
    }

    public T Get<T>() where T : class, IState
    {
        if (_states.TryGetValue(typeof(T), out var s)) return (T)s;
        Debug.LogError($"State not registered: {typeof(T).Name}");
        return null;
    }

    public void SetInitial<T>() where T : class, IState => Set(Get<T>());


    public void AddAnyTransition<TTo>(Func<bool> when) where TTo : class, IState
    {
        var to = Get<TTo>();
        _anyTransitions.Add(new Transition(when, to));
    }

}






public static class SteeringHelpers
{
    public static Vector3 MoveTo(Transform self, Transform target)
    {
        return (target.position);
    }


    public static Vector3 KeepRange(Transform self, Transform target, float min, float max)
    {
        if (target == null) return self.transform.position;
        var to = target.position - self.position;
        float d = to.magnitude;
        float optimal = (min + max) * 0.5f;
        if (d < 0.001f) return self.transform.position;
        if(d > max || d < min) return ((target.position - optimal * to.normalized));
        else { return self.transform.position; }

    }

}

public abstract class EnemyStateInstaller : MonoBehaviour
{
    public abstract void Install(EAI_StateMachine sm, EnemyContext ctx);
}
