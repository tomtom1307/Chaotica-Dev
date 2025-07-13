using System.Collections.Generic;
using UnityEngine;

public class HexLevelGenerator : MonoBehaviour
{
    [Header("Prefabs & Settings")]
    public GameObject roomPrefab;
    public float hexSize = 5f;       // center → corner
    public int mainPathLength = 10;

    // Pointy-top axial neighbor offsets (face adjacency)
    static readonly Vector2Int[] directions = {
        new Vector2Int(+1,  0), // East
        new Vector2Int(+1, -1), // Northeast
        new Vector2Int( 0, -1), // Northwest
        new Vector2Int(-1,  0), // West
        new Vector2Int(-1, +1), // Southwest
        new Vector2Int( 0, +1), // Southeast
    };

    HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();

    void Start()
    {
        // carve out the main path
        List<Vector2Int> mainPath = BuildPath(Vector2Int.zero, mainPathLength);

        // instantiate every occupied cell
        foreach (var cell in occupied)
        {
            Vector3 worldPos = AxialToWorld(cell);
            Instantiate(roomPrefab, worldPos, Quaternion.identity, transform);
        }
    }

    /// <summary>
    /// Walks from `start`, carving exactly `length` steps,
    /// always choosing an unoccupied face-adjacent neighbor.
    /// </summary>
    List<Vector2Int> BuildPath(Vector2Int start, int length)
    {
        var path = new List<Vector2Int> { start };
        occupied.Add(start);
        var current = start;

        for (int i = 1; i < length; i++)
        {
            // collect all face-neighbors that aren’t already used
            var free = new List<Vector2Int>();
            foreach (var d in directions)
            {
                var candidate = current + d;
                if (!occupied.Contains(candidate))
                    free.Add(candidate);
            }

            if (free.Count == 0)
            {
                Debug.LogWarning($"[%] Stopped at length {i} — no free neighbors left.");
                break;
            }

            // pick one at random
            current = free[Random.Range(0, free.Count)];
            path.Add(current);
            occupied.Add(current);
        }

        return path;
    }

    // Pointy-top axial (q,r) → Unity world (x,z)
    Vector3 AxialToWorld(Vector2Int hex)
    {
        // hexSize = distance from center to any corner
        float x = hexSize * Mathf.Sqrt(3f) * (hex.x + hex.y * 0.5f);
        float z = hexSize * (1.5f * hex.y);
        return new Vector3(x, 0f, z);
    }

}
