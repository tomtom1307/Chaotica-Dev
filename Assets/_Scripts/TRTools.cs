using UnityEngine;

namespace TRTools
{
    public static class VecOp
    {
        public static Vector3 Direction(Vector3 A, Vector3 B)
        {
            return (A - B).normalized;
        }

        public static Vector3 Direction(Transform A, Transform B)
        {
            return (A.position - B.position).normalized;
        }

        public static Vector3 RandomDir(Vector3 A, Vector3 B)
        {
            float X = Random.Range(A.x, B.x);
            float Y = Random.Range(A.y, B.y);
            float Z = Random.Range(A.z, B.z);

            return new Vector3(X, Y, Z).normalized;
        }



    }
}