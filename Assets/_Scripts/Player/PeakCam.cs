using UnityEngine;

public class PeakCam : MonoBehaviour
{
    [SerializeField] LayerMask Layer;
    [SerializeField] Transform HeadPos;
    [SerializeField] Transform Orientation;
    [SerializeField] float XOffset;
    [SerializeField] float YOffset;
    [SerializeField] float Rot;
    [SerializeField] float Speed;
    [SerializeField] float MoveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        orginal = HeadPos.localPosition;
        pm = GetComponent<PlayerMovement>();
    }

    Vector3 targetPos;
    Quaternion TargetZ;
    Vector3 orginal;
    PlayerMovement pm;
    // Update is called once per frame
    void Update()
    {
        if (pm.state == PlayerMovement.PlayerMechanimState.Sliding) return;
        if (Input.GetKey(KeyCode.Q))
        {
            MoveHead(-1);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            MoveHead(1);
            
        }
        else
        {
            
            targetPos = orginal;
            TargetZ = Quaternion.Euler(0, 0, 0);
            if (pm.wh.State != WeaponHolder.AttackState.Ready)
            {
                
            }
            else
            {
                pm.ResetMoveSpeed();
            }
        }


        HeadPos.localPosition = Vector3.Lerp(HeadPos.localPosition, targetPos, Speed * Time.deltaTime);
        HeadPos.localRotation = Quaternion.Lerp(HeadPos.localRotation, TargetZ, Speed * Time.deltaTime);
    }


    public void MoveHead(float PM)
    {
        RaycastHit hit;
        
        if(Physics.Raycast(HeadPos.position, PM * Orientation.right, out hit, 1.1f*XOffset))
        {
            if(hit.collider != null)
            {
                targetPos =Vector3.up* orginal.y+HeadPos.InverseTransformPoint(hit.point);
            }
        }
        else
        {
            targetPos = orginal + new Vector3(PM * XOffset, -YOffset, 0);
            
        }
        TargetZ = Quaternion.Euler(0, 0, -PM * Rot);
        pm.SetMoveSpeed(0.5f);
        
    }
}
