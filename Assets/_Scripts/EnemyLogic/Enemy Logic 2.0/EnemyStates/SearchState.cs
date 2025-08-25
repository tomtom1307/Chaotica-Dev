using UnityEngine;

public class SearchState : BaseState
{
    EnemyContext c;

    SearchPhase phase;
    float StartedAt;
    Vector3 anchor;
    int lookIndex;
    float nextLookAtTime;
    int ringStepIndex;
    private Vector3 currentStepTarget;
    float Speed => c.cfg.approachSpeed * c.cfg.searchSpeedMult;
    enum SearchPhase { Travel, Scan, Done}
    public float focusApproachSpeedMult = 0.5f; // gentle drift toward player while filling meter
    public SearchState(EnemyContext ctx)
    {
        this.c = ctx;
    }

    public override IState Next()
    {
        // If player is seen again, go to combat
        if (c.bb.DetectionMeter == 1 && c.bb.hasLOS)
        {
            
            return new KeepRangeState(c);
        }

        // Go back to Idle
        if (phase == SearchPhase.Done)
        {
            return new IdleState(c);
        }

        return null;
    }

    public override void OnEnter()
    {
        StartedAt = Time.time;
        phase = SearchPhase.Travel;
        anchor = c.bb.lastKnownTargetPos;
        lookIndex = 0;
        nextLookAtTime = 0;
        ringStepIndex = 0;
        currentStepTarget = anchor;
    }

    public override void OnExit()
    {
        
    }

    public override void Tick()
    {
        
        bool timeUp = Time.time - StartedAt >= c.cfg.SearchDuration;
        switch (phase)
        {
            case SearchPhase.Travel:
                {
                    // Move to last-known pos
                    Vector3 toAnchor = anchor - c.transform.position;
                    Vector3 vel = toAnchor.sqrMagnitude > 0.0001f
                        ? new Vector3(toAnchor.normalized.x, 0f, toAnchor.normalized.z) * Speed
                        : Vector3.zero;

                    c.motor.SetVelocity(vel);
                    c.motor.SetAltitude(c.cfg.altitudeOffset, c.bb.target); // flyers hover near player height
                                                                            // Face movement (or look at anchor)
                    if (toAnchor.sqrMagnitude > 0.01f) c.aimer.AimAt(c.transform.position + new Vector3(vel.x, 0, vel.z));

                    // Arrived?
                    if (toAnchor.magnitude <= c.cfg.searchArrivalDistance)
                    {
                        phase = SearchPhase.Scan;
                        ScheduleNextLook();
                        PickNextRingStepPoint(); // optional first step
                    }
                    break;
                }

            case SearchPhase.Scan:
                {
                    if (c.bb.hasLOS)
                    {
                        phase = SearchPhase.Travel;
                        anchor = c.bb.lastKnownTargetPos;
                    }
                    else
                    {
                        // 1) Small ring step movement (optional)
                        Vector3 stepVel = Vector3.zero;
                        if (c.cfg.searchRingSteps > 0 && ringStepIndex < c.cfg.searchRingSteps)
                        {
                            Vector3 toStep = currentStepTarget - c.transform.position;
                            if (toStep.magnitude <= 0.35f)
                            {
                                ringStepIndex++;
                                PickNextRingStepPoint();
                            }
                            else
                            {
                                var dir = new Vector3(toStep.normalized.x, 0f, toStep.normalized.z);
                                stepVel = dir * (Speed * 0.6f); // slow sidestep while scanning
                            }
                        }
                        c.motor.SetVelocity(stepVel);
                        c.motor.SetAltitude(c.cfg.altitudeOffset, c.bb.target);

                        // 2) Head/torso “look” sweep over segments
                        if (Time.time >= nextLookAtTime)
                        {
                            lookIndex = (lookIndex + 1) % Mathf.Max(1, c.cfg.searchLookSegments);
                            ScheduleNextLook();
                        }

                        // Aim toward the current segment direction
                        Vector3 lookDir = LookDirection(lookIndex);
                        c.aimer.AimAt(c.transform.position + lookDir * 5f);


                        if (c.bb.hasLOS)
                        {

                        }
                        // Done?
                        if (timeUp) phase = SearchPhase.Done;
                        
                    }
                    break;
                }

            case SearchPhase.Done:
                {
                    
                    c.motor.SetVelocity(Vector3.zero);
                    c.motor.SetAltitude(c.cfg.altitudeOffset, c.bb.target);
                    break;
                }
        }



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
