using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EAI_AnimatorController : MonoBehaviour
{
    public float MoveScaleFactor = 1;
    public float SpeedDamp = 0.12f; //For smoother blendtree 
    [HideInInspector] public Animator animator;
    [HideInInspector] public EnemyContext ctx;
    

    // Animator param hashes
    static readonly int HashMoveBlend = Animator.StringToHash("MoveBlend");
    static readonly int HashHit = Animator.StringToHash("Hit");
    static readonly int HashStunned = Animator.StringToHash("IsStunned");


    private void Awake()
    {
        if (!TRTools.Helpers.TryFind<Animator>(gameObject, out animator))
        {
            Debug.LogWarning("Enemy does not have an animator please place one");
        }
        ctx =GetComponent<EnemyContext>();
        
    }

    EAI_AnimationEventHandler eventHandler;
    private void Start()
    {

        eventHandler = GetComponentInChildren<EAI_AnimationEventHandler>();
        ApplyRootMotion(false);
    }


    private void Update()
    {
        Vector3 vel = ctx.motor.Velocity;
        float VelocityAligned = Vector3.Dot(vel, transform.forward);
        float MaxSpeed = ctx.motor.MaxSpeed;
        float Speed01 = (MaxSpeed > 0.01f) ? Mathf.Clamp(VelocityAligned / MaxSpeed,-1,1) : 0f;
        float MoveBlend = MoveScaleFactor * Speed01;

        SetMoveBlend(MoveBlend);
    }

    public void SetMoveBlend(float BLEND)
    {
        if (!animator) return;
        animator.SetFloat(HashMoveBlend, BLEND, SpeedDamp, Time.deltaTime);
    }

    public void TriggerHit()
    {
        if (!animator) return;
        animator.ResetTrigger(HashHit);
        animator.SetTrigger(HashHit);
    }

    public void Stunned(bool x)
    {
        if (!animator) return;
        animator.enabled = x;
    }

    public void ApplyRootMotion(bool x)
    {
        if(!eventHandler) return;
        eventHandler.ApplyRootMotion = x;
    }

    public void PlayAttack(int AttackInt)
    {
        if (!animator) return;
        animator.SetBool("Attack", true);
        animator.SetInteger("AttackInt", AttackInt);
    }

    public void ResetAttackAnim()
    {
        if (!animator) return;
        animator.SetBool("Attack", false);
    }


    // If you ever enable root motion, push it through your motor here
    void OnAnimatorMove()
    {
        if (!animator || !animator.applyRootMotion || ctx?.motor == null) return;

        
        Vector3 delta = animator.deltaPosition;
        Vector3 desiredVel = delta / Mathf.Max(Time.deltaTime, 0.0001f);

        // Feed your PID / motor (pseudo-code, adapt to your motor API)
        ctx.motor.MoveTo(delta);
    }
}
