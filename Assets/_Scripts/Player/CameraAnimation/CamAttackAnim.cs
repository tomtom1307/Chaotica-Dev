using NUnit.Framework.Internal.Execution;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class CamAttackAnim : MonoBehaviour
{
    public Vector2 SwingDir;
    WeaponHolder weaponHolder;

    public void DoAnim(SwingDirection direction)
    {
        //Take enum value and swing in direction
        switch (direction)
        {
            case SwingDirection.Left:
                {
                    SwingDir = Vector2.left;
                    break;
                }
            case SwingDirection.Right:
                {
                    SwingDir = Vector2.right;
                    break;
                }
            case SwingDirection.Up:
                {
                    SwingDir = Vector2.up;
                    break;
                }
            case SwingDirection.Down:
                {
                    SwingDir = Vector2.down;
                    break;
                }
            case SwingDirection.UR:
                {
                    SwingDir = (Vector2.right + Vector2.up).normalized;
                    break;
                }
            case SwingDirection.UL:
                {
                    SwingDir = (Vector2.left + Vector2.up).normalized;
                    break;
                }
            case SwingDirection.DL:
                {
                    SwingDir = (Vector2.left + Vector2.down).normalized;
                    break;
                }
            case SwingDirection.DR:
                {
                    SwingDir = (Vector2.right + Vector2.down).normalized;
                    break;
                }
        }

        RotateCamera(SwingDir, weaponHolder.CurrentAttackData.AttackWeight * weaponHolder.ChargeAmount);


    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateCamera(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateCamera(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            RotateCamera(Vector2.up+Vector2.left);
        }*/
    }

    public float rotationSpeed = 2f;  // Speed of rotation
    public float rotationMagnitude = 15f;  // Max rotation angle

    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.rotation;
        weaponHolder = GetComponent<CameraController>().Player.GetComponent<WeaponHolder>();
    }

    public void RotateCamera(Vector2 direction, float Mult)
    {
        //StopAllCoroutines();
        StartCoroutine(RotateRoutine(direction, Mult));
    }

    private IEnumerator RotateRoutine(Vector2 direction, float Mult)
    {
        Quaternion targetRotation = Quaternion.Euler(
            -direction.y * rotationMagnitude * Mult,
            direction.x * rotationMagnitude * Mult,
            0f
        ) * originalRotation;

        // Rotate towards target
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed * 1.5f * Mult;
            yield return null;
        }

        // Ensure exact target rotation
        transform.rotation = targetRotation;

        // Rotate back to original
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed*1;
            yield return null;
        }

        transform.rotation = originalRotation;
    }
}
