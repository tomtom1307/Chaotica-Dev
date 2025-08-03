using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace Project
{
    
    public class CamShake : MonoBehaviour
    {
        private Vector3 StartPos;
        const float maxAngle = 10f;
        public CamShakeProperties testProps;
        public CamShakeProperties onHit;
        public CamShakeProperties onAttack;
        public CamShakeProperties onAOE;
        public CamShakeProperties onDestroy;
        public static CamShake instance;
        IEnumerator currentShakeCorutine;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Destroy(this);

            StartPos = transform.localPosition;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Alpha3))
            {
                StartShake(testProps);
            }
        }


        public void StartShake(CamShakeProperties properties)
        {
            print("DID CAMSHAKE");
            if(currentShakeCorutine != null) {
                StopCoroutine(currentShakeCorutine);
            }
            currentShakeCorutine = Shake(properties);
            StartCoroutine(currentShakeCorutine);
        }

        IEnumerator Shake(CamShakeProperties properties)
        {
            float completePercent = 0;
            float MovePercent = 0;

            float angle_radians = properties.angle*Mathf.Deg2Rad - Mathf.PI;
            Vector3 previousWaypoint = StartPos;
            Vector3 currentWayPoint = StartPos;
            float moveDistance = 0;

            Quaternion targetRot = Quaternion.identity;
            Quaternion previousRot = Quaternion.identity;

            do
            {
                if (MovePercent >= 1 || completePercent == 0)
                {
                    float NoiseAngle = (UnityEngine.Random.value - 0.5f) * Mathf.PI;
                    float DampingFactor = DampingCurve(completePercent, properties.DampedPercent);
                    angle_radians += Mathf.PI + NoiseAngle * properties.NoisePercent;
                    currentWayPoint = StartPos + new Vector3(Mathf.Cos(angle_radians), Mathf.Sin(angle_radians)) * properties.strenght * DampingFactor;
                    previousWaypoint = transform.localPosition;
                    moveDistance = Vector3.Distance(currentWayPoint, previousWaypoint);

                    targetRot = Quaternion.Euler(new Vector3(currentWayPoint.y, currentWayPoint.x).normalized * properties.rotationPercent * DampingFactor * maxAngle);
                    previousRot = transform.localRotation;

                    MovePercent = 0;
                }

                completePercent += Time.deltaTime / properties.duration;
                MovePercent += Time.deltaTime / moveDistance * properties.speed;
                transform.localPosition = Vector3.Lerp(previousWaypoint, currentWayPoint, MovePercent);
                transform.localRotation = Quaternion.Slerp(previousRot, targetRot, MovePercent);

                yield return null;
            } while (moveDistance > 0);



        }

        float DampingCurve(float x, float dampingPercent)
        {
            x = Mathf.Clamp01(x);
            float a = Mathf.Lerp(2, .25f, dampingPercent);
            float b = 1 - Mathf.Pow(x, a);
            return b * b * b;
        }


        [Serializable]
        public class CamShakeProperties
        {
            public float angle;
            public float strenght;
            public float speed;
            public float duration;
            [Range(0, 1)]
            public float NoisePercent;
            [Range(0, 1)]
            public float DampedPercent;
            [Range(0, 1)]
            public float rotationPercent;
        }
    }
}
