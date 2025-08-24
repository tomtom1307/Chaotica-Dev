using UnityEngine;

[CreateAssetMenu(menuName = "AI/Abilities/FireBall")]
public class FireballAbility : AbilitySO
{
    public GameObject projectilePrefab;
    public float Speed;
    public float spreadDeg = 2f;

    public override void Execute(GameObject owner, in AbilityContext ctx)
    {
        if (!projectilePrefab) return;
        var dir = Quaternion.Euler(Random.Range(-spreadDeg, spreadDeg),
                                   Random.Range(-spreadDeg, spreadDeg), 0f) * ctx.direction.normalized;
        Instantiate(projectilePrefab, ctx.origin, Quaternion.LookRotation(dir, Vector3.up));
    }
}
