using UnityEngine;

public class SearchState : BaseState
{
    

    SearchPhase phase;
    public float StartedAt;
    Vector3 anchor;
    int lookIndex;
    float nextLookAtTime;
    int ringStepIndex;
    private Vector3 currentStepTarget;
    float Speed => c.cfg.approachSpeed * c.cfg.searchSpeedMult;

    public override IState.StateAggroType stateAggroType => IState.StateAggroType.Search;

    enum SearchPhase { Travel, Scan, Done, Stare}
    public float focusApproachSpeedMult = 0.5f; // gentle drift toward player while filling meter
    public SearchState(EnemyContext c) : base(c) { }


    public override void OnEnter()
    {
        c.bb.Search = false;
        StartedAt = Time.time;
        if (c.bb.InvestigateSound)
        {
            anchor = c.bb.POI;
            c.bb.InvestigateSound = false;
        }
        else
        {
            anchor = c.bb.lastKnownTargetPos;
        }

        phase = SearchPhase.Travel;

        lookIndex = 0;
        nextLookAtTime = 0;
        ringStepIndex = 0;
        currentStepTarget = anchor;
        base.OnEnter();
    }

    public override void OnExit()
    {
        c.bb.POI = c.bb.lastKnownTargetPos;
        c.bb.InvestigateSound = false;
        c.bb.Search = false;
    }
    public bool timeUp;
    public override void Tick()
    {
        timeUp = Time.time - StartedAt >= c.cfg.SearchDuration;
        if (c.bb.hasLOS) { OnEnter(); phase = SearchPhase.Stare; }

        if (c.bb.InvestigateSound && anchor != c.bb.POI)
        {
            OnEnter();
        }
        switch (phase)
        {
            case SearchPhase.Travel:
                {
                    c.motor.MoveTo(anchor);
                    c.aimer.AimAt(anchor);
                    break;
                }
            case SearchPhase.Scan:
                {
                    
                    break;
                }
            case SearchPhase.Stare:
                {
                    c.aimer.AimAt(c.bb.target.position);
                    c.motor.MoveTo(SteeringHelpers.KeepRange(c.transform, c.bb.target,c.cfg.preferredMin, c.cfg.preferredMax));
                    break;
                }

            case SearchPhase.Done:
                {
                    
                    c.motor.SetVelocity(Vector3.zero);
                    c.motor.SetAltitude(c.cfg.DefaultAltitude, c.bb.target);
                    break;
                }
        }
        if (timeUp) c.bb.InvestigateSound = false;
    }


    void ScheduleNextLook() => nextLookAtTime = Time.time + Mathf.Max(0.05f, c.cfg.searchDwell);

    Vector3 LookDirection(int idx)
    {
        // Spread evenly around full 360°
        int segments = Mathf.Max(1, c.cfg.searchLookSegments);  
        float ang = (idx / (float)segments) * Mathf.PI * 2f;
        Vector3 dir = new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang));
        return dir;
    }

    void PickNextRingStepPoint()
    {
        if (c.cfg.searchRingSteps <= 0) { currentStepTarget = anchor; return; }
        float t = ringStepIndex / Mathf.Max(1f, c.cfg.searchRingSteps);
        float ang = t * Mathf.PI * 2f;
        Vector3 offset = new Vector3(Mathf.Cos(ang), 0, Mathf.Sin(ang)) * c.cfg.SearchRingRadius;
        currentStepTarget = anchor + offset;
    }


}
