using UnityEngine;

public class HandIKHandler : MonoBehaviour
{
    [Header("IK Target")]
    public Transform IKTarget_L;
    public Transform IKTarget_R;

    public Transform R_Desired;
    public Transform L_Desired;

    [Header("IK Pole")]
    public Transform R_Pole;
    public Transform L_Pole;

    public Transform R_Pole_Desired;
    public Transform L_Pole_Desired;



    public void GetWeaponIkPos()
    {
        WeaponIKTargets weaponIKTargets = GetComponentInChildren<WeaponIKTargets>();
        L_Desired = weaponIKTargets.LeftHand;
        R_Desired = weaponIKTargets.RightHand;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ForceUpdate();
        }
        if (L_Desired != null)
        {
            IKTarget_L.position = L_Desired.position;
            IKTarget_L.rotation = L_Desired.rotation;
        }
        if (R_Desired != null)
        {
            IKTarget_R.position = R_Desired.position;
            IKTarget_R.rotation = R_Desired.rotation;
        }
    }

    public void ForceUpdate()
    {
        Debug.Log("Forced IK Update");
        if (L_Desired != null)
        {
            IKTarget_L.position = L_Desired.position;
            IKTarget_L.rotation = L_Desired.rotation;
        }
        if (R_Desired != null)
        {
            IKTarget_R.position = R_Desired.position;
            IKTarget_R.rotation = R_Desired.rotation;
        }
    }

}
