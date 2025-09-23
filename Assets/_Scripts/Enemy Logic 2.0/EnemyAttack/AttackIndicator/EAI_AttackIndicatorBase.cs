using System.Collections;
using UnityEngine;

public class EAI_AttackIndicatorBase : MonoBehaviour
{
    EnemyContext ctx;
    public virtual void Start()
    {
        ctx = GetComponent<EnemyContext>();
    }

    public virtual void isActive(bool x)
    {

    }

    public IEnumerator IndicatorFlash(float FlashTime)
    {
        isActive(false);
        yield return new WaitForSeconds(FlashTime);
        isActive(true);
    }


    public virtual void SetIndicatorGlow(float Lerp)
    {
        
    }

    public virtual void SetPosition(Vector3 targetPos) { }
}
