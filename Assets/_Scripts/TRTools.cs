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
    }
}