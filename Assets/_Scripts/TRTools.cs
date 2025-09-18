using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
            //TODO when can be fucked 
            return (B - A).normalized;
        }

        public static Vector3 Direction(Transform A, Transform B)
        {
            return (A.position - B.position).normalized;
        }

        public static Vector3 RandomDir(Vector3 A, Vector3 B)
        {
            float X = UnityEngine.Random.Range(A.x, B.x);
            float Y = UnityEngine.Random.Range(A.y, B.y);
            float Z = UnityEngine.Random.Range(A.z, B.z);

            return new Vector3(X, Y, Z).normalized;
        }



    }

    public static class floatOP
    {
        public static bool InRange(float x, float min, float max)
        {
            if(x>min && x< max) return true;
            else return false;
        }
    }


    public static class Helpers
    {
        public static bool TryFind<T>(GameObject go, out T comp) where T : class
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

    public static class NavMeshUtil
    {
        /// Returns the closest point *on the NavMesh* near `worldPos`.
        public static bool TryGetNearestOnNavMesh(
            Vector3 worldPos,
            float maxSearchDistance,
            out Vector3 nearest,
            int areaMask = NavMesh.AllAreas)
        {
            if (NavMesh.SamplePosition(worldPos, out var hit, maxSearchDistance, areaMask))
            {
                nearest = hit.position; // already on-mesh, with proper Y
                return true;
            }

            nearest = worldPos;
            return false;
        }


        public static bool IsOnNavmesh(Vector3 pos ,float Range)
        {
            if (TryGetNearestOnNavMesh(pos, Range, out Vector3 near))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    public static class Weighted_Distribution
    {

        //Example use -------- var Chosen = Sample_Weighted_Distribution<AbilityEntry>(ValidAttacks, e => e.Weight);
        public static T Sample_Weighted_Distribution<T>(IList<T> items, Func<T, float> weight, System.Random rng = null)
        {
            if (items == null || items.Count == 0) throw new ArgumentException("Empty list");

            float total = 0;
            for (int i = 0; i < items.Count; i++)
            {
                total += Mathf.Max(0, weight(items[i])); //Clamps negatives
            }

            if(total <= 0)
                return items[UnityEngine.Random.Range(0, items.Count-1)];

            //Generate RN
            double r = (rng != null ? rng.NextDouble() : UnityEngine.Random.value) * total;

            //Evaluate
            for (int i = 0; i < items.Count; i++)
            {
                r -= Mathf.Max(0f, weight(items[i]));
                if (r <= 0.0) return items[i];
            }

            return items[items.Count - 1]; //Default option

        }
    }


}