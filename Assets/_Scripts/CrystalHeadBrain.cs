using UnityEngine;

public class CrystalHeadBrain : EnemyBrain
{
    public override void MapBools()
    {
        base.MapBools();
    }

    public override void MapFloats()
    {
        base.MapFloats();
    }

    public override void MapInts()
    {
        base.MapInts();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (navMesh.enabled)
        {
            if (navMesh.remainingDistance == 0) animator.SetBool("Walking", false);
            if (animator.GetBool("Walking")) animator.SetFloat("WalkSpeed", navMesh.velocity.magnitude);
        }
        
    }
}
