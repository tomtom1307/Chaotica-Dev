using UnityEngine;

public enum BuffStackMode { RefreshDuration, StackDuration, Independent }
public enum BuffType { Additive, Multiplicative }
public abstract class statusEffectBase
{
    public float Duration;
    public BuffStackMode StackMode;
    public BuffType BuffType;

    protected float remainingTime;
    protected EnemyBrain enemy;
    protected GameObject obj;
    protected PlayerHealth player;
    public void StartEffect(GameObject Object)
    {
        remainingTime = Duration;
        obj = Object;
        onApply(obj);
    }

    public virtual void UpdateEffect(float delta)
    {
        remainingTime -= delta;
        if(IsEffectDone())
        {
            OnExpire(obj);
        }
    }

    public bool IsEffectDone()
    {
        if(remainingTime <= 0) return true;
        else return false;
    }


    protected abstract void onApply(GameObject Object);
    protected abstract void OnExpire(GameObject Object);

    

}
