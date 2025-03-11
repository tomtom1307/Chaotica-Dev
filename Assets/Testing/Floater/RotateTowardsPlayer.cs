using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RotateTowardsPlayer : MonoBehaviour
{
    public EnemyBrain brain;

    public Transform player;
    public Rigidbody Playerrb;
    public float rotationSpeed = 2.0f; 

    private void Start()
    {
        this.enabled = false;
    }

    public void EnableScript(EnemyBrain brain)
    {
        this.enabled = true;
        this.brain = brain;
        player = brain.perception.player;
        Playerrb = player.GetComponent<Rigidbody>();
    }

    public float maxOvershoot = 1;
    private float t = 0;

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position+ Vector3.up + Vector3.ClampMagnitude(Playerrb.linearVelocity, maxOvershoot) - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);


        // Lerp towards the overshot rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
