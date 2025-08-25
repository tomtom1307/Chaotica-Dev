using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoverMotor : MonoBehaviour, IMotor
{
    public float maxSpeed = 12f;
    public float maxAccel = 40f;
    public float drag = 0.5f;
    public float hoverP = 7.5f, hoverD = 3.0f, maxUpForce = 60f;

    Rigidbody rb;
    Vector3 desiredXZ;
    float? desiredAlt; Transform altRef;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.linearDamping = drag;
    }

    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public bool Enabled { get => enabled; set => enabled = value; }

    public void SetVelocity(Vector3 v) { v.y = 0f; desiredXZ = Vector3.ClampMagnitude(v, maxSpeed); }
    public void SetAltitude(float? h, Transform rel) { desiredAlt = h; altRef = rel; }

    void FixedUpdate()
    {
        // XZ steer
        Vector3 velXZ = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Vector3 dv = desiredXZ - velXZ;
        Vector3 accel = Vector3.ClampMagnitude(dv / Time.fixedDeltaTime, maxAccel);
        rb.AddForce(accel * rb.mass, ForceMode.Force);

        // Hover Y
        if (desiredAlt.HasValue)
        {
            float refY = altRef ? altRef.position.y : 0f;
            float alt = transform.position.y - refY;
            float err = desiredAlt.Value - alt;
            float dterm = -rb.linearVelocity.y;
            float up = Mathf.Clamp((err * hoverP + dterm * hoverD) * rb.mass, -maxUpForce, maxUpForce);
            rb.AddForce(Vector3.up * up, ForceMode.Force);
        }
    }
}

