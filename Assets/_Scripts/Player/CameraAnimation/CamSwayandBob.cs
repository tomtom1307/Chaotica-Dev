using System;
using UnityEngine;

public class CamSwayandBob : MonoBehaviour
{
    [SerializeField] PlayerMovement pm;
    [SerializeField] CameraController cc;
    Rigidbody rb;
    private void Start()
    {
        rb = pm.GetComponent<Rigidbody>();
    }

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
        BobRotation();


        CompositePositionRotation();
    }

    

    float smooth = 10;
    float smoothRot = 12;
    private void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos+bobPosition, Time.deltaTime * smooth);


        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot)* Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }

    
    Vector2 walkInput;
    Vector2 lookInput;

    private void GetInput()
    {
        walkInput = pm.moveInput;
        lookInput = cc.mouseInput;
    }

    [Header("SwayPos")]
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

    [Header("SwayRot")]
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
    [Header("BobOffset")]
    public float SpeedCurve;
    float curveSin { get => Mathf.Sin(SpeedCurve); }
    float curveCos { get => Mathf.Cos(SpeedCurve); }

    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;

    Vector3 bobPosition;
    private void BobOffset()
    {
        SpeedCurve += Time.deltaTime * (pm.isGrounded ? rb.linearVelocity.magnitude : 1) + 0.01f;

        if(bobOffset == false) { bobPosition = Vector3.zero; return; }

        bobPosition.x = (curveCos * bobLimit.x * (pm.isGrounded ? 1 : 0)) - (walkInput.x * travelLimit.x);

        bobPosition.y = (curveSin*bobLimit.y) - (rb.linearVelocity.y * travelLimit.y);

        bobPosition.z = -(walkInput.y * travelLimit.z);
    }

    [Header("BobRot")]
    public Vector3 multiplier;
    Vector3 bobEulerRotation;
    private void BobRotation()
    {
        if(bobSway == false) { bobEulerRotation = Vector3.zero; return; }

        bobEulerRotation.x = (walkInput != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * SpeedCurve)) : 
                                                            multiplier.x * Mathf.Sin(2 * SpeedCurve) / 2);
        bobEulerRotation.y = (walkInput != Vector2.zero ? multiplier.y * curveCos:0);
        bobEulerRotation.z = (walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0);

    }


}
