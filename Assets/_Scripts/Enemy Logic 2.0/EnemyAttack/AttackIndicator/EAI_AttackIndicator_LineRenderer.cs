using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EAI_AttackIndicator_LineRenderer : EAI_AttackIndicatorBase
{
    LineRenderer lr;
    public override void Start()
    {
        base.Start();
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);
        mat = GetComponent<Renderer>().material;
    }
    private Material mat;
    public override void isActive(bool x)
    {
        base.isActive(x);
        lr.enabled = x;
    }

    public override void SetIndicatorGlow(float Lerp)
    {

        Lerp = Mathf.Clamp01(Lerp);
        mat.SetFloat("_Lerp", Lerp);
    }

    public override void SetPosition(Vector3 targetPos)
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, targetPos);

    }
}
