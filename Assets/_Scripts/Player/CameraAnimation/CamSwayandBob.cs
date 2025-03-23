using System;
using UnityEngine;

public class CamSwayandBob : MonoBehaviour
{
    [SerializeField] PlayerMovement pm;
    [SerializeField] CameraController cc;

    public bool sway = true;
    public bool swayRotation = true;
    public bool bobOffset = true;
    public bool bobSway = true;


    private void Update()
    {
        GetInput();

        Sway();
        SwayRotation();
        BobOffset();
        BobSway();


        CompositePositionRotation();
    }

    private void BobOffset()
    {
        
    }

    float smooth = 10;
    float smoothRot = 12;
    private void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos, Time.deltaTime * smooth);


        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot), Time.deltaTime * smoothRot);
    }

    private void BobSway()
    {
        
    }

    Vector2 walkInput;
    Vector2 lookInput;

    private void GetInput()
    {
        walkInput = pm.moveInput;
        lookInput = cc.mouseInput;
    }

    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;
    //Handles position Change
    private void Sway()
    {
        if(sway == false) { swayPos = Vector3.zero; return; }

        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;

    }


    public float rotationStep = 4;
    public float maxRotationStep = 5;
    Vector3 swayEulerRot;
    private void SwayRotation()
    {
        if (swayRotation == false) { swayEulerRot = Vector3.zero; return; }

        Vector2 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);

        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    
}
