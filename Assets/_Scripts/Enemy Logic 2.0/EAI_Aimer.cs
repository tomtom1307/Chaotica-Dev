using UnityEngine;

public class Aimer : MonoBehaviour
{
    public bool flatRotation = true;
    public bool usePhysics = true;
    public float turnSpeedDegPerSec = 360f;
    float defaultSpeed;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb)
        {
            if (usePhysics && !rb.isKinematic)
            {
                rb.freezeRotation = false;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }
            else rb.freezeRotation = true;
        }
        
    }

    private void Start()
    {
        if(defaultSpeed == 0) defaultSpeed = turnSpeedDegPerSec;
    }

    // call this from your other script; pass dt
    public void AimAt(Vector3 worldPoint)
    {
        float dt;
        if (usePhysics)
        {
            dt = Time.fixedDeltaTime;
        }
        else
        {
            dt = Time.deltaTime;
        }
            Vector3 origin = (rb && usePhysics && !rb.isKinematic) ? rb.position : transform.position;
        Vector3 to = worldPoint - origin;
        if (flatRotation) to.y = 0f;
        if (to.sqrMagnitude < 1e-6f) return;

        Quaternion target = Quaternion.LookRotation(to.normalized, Vector3.up);
        float step = Mathf.Max(0f, turnSpeedDegPerSec) * dt;

        if (rb && usePhysics && !rb.isKinematic)
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, target, step));
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, step);
    }

    public void AimAt(Vector3 worldPoint, float RotSpeed)
    {
        turnSpeedDegPerSec = RotSpeed;
        AimAt(worldPoint);
    }

    public void ResetSpeedToDefault()
    {
        turnSpeedDegPerSec = defaultSpeed;
    }

    public void SetDefaultRotSpeed(float val)
    {
        defaultSpeed = val;
    }

    public void AimSnap(Vector3 worldPoint)
    {
        Vector3 origin = (rb && usePhysics && !rb.isKinematic) ? rb.position : transform.position;
        Vector3 to = worldPoint - origin;
        if (flatRotation) to.y = 0f;
        if (to.sqrMagnitude < 1e-6f) return;

        Quaternion target = Quaternion.LookRotation(to.normalized, Vector3.up);
        if (rb && usePhysics && !rb.isKinematic) rb.MoveRotation(target);
        else transform.rotation = target;
    }
}
