using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(LineRenderer))]
public class LaserPredictionLR : MonoBehaviour
{
    LineRenderer lr;

    EnemyBrain EB;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        
        EB = GetComponentInParent<EnemyBrain>();
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        lr.SetPosition(0, transform.position);

        Ray ray = new Ray(transform.position, EB.attackHandler.AimDirection.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + transform.forward * 100f);
        }

    }
}
