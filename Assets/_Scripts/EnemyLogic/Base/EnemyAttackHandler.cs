using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    [HideInInspector] public EnemyBrain brain;
    [HideInInspector] public EnemyAttack currentAttack;
    public bool attacking;
    public EnemyAttackHandler(EnemyBrain brain)
    {
        this.brain = brain;
    }

    public void EnterAttack()
    {
        attacking = true;
        brain.animator.SetInteger("AT", currentAttack.attackType);
        currentAttack.attackData.EnterAttack(this);

        brain.animator.SetBool("Attacking", attacking);
    }
    public void ExitAttack()
    {
        attacking = false;
        currentAttack.attackData.ExitAttack(this);

        brain.animator.SetBool("Attacking", attacking);
    }

    public void DoRayCast() { }
    public void DoProjectile() { }

    public void DoColliderCheck() { }

}
