using UnityEngine;

namespace TRTools
{
    public static class VecOp
    {

        public static Vector3 DirectionDistance(Vector3 A, Vector3 B)
        {
            return (A - B);
        }

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

    public static class Helpers
    {
        public static bool TryFind<T>(GameObject go, out T comp) where T : Component
        {
            if(go.TryGetComponent<T>(out comp))
            {
                return true;
            }
            comp = go.GetComponentInParent<T>();
            if (comp != null) return true;
            comp = go.GetComponentInChildren<T>();
            if(comp != null) return true;
            else return false;
            
        }
 
    }
}