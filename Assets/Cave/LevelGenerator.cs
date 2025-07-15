using System.Collections.Generic;
using UnityEngine;

public class HexLevelGenerator : MonoBehaviour
{
    [Header("Prefabs & Settings")]
    public GameObject roomPrefab;
    public GameObject roomStorePrefab;
    public float hexSize = 5f;       // center → corner

    [Header("Path & Store Settings")]
    public int mainPathLength = 12;      // total rooms (including start)
    [Tooltip("Place a store every N rooms along the main path (skip the start).")]
    public int storeInterval = 4;

    // Axial neighbor offsets
    static readonly Vector2Int E = new Vector2Int(+1, 0);
    static readonly Vector2Int NE = new Vector2Int(+1, -1);
    static readonly Vector2Int SE = new Vector2Int(0, +1);

    HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();

    void Start()
    {
        // 1) Compute store anchors along the pure‐east axis
        var anchors = new List<Vector2Int> { Vector2Int.zero };
        for (int i = storeInterval; i < mainPathLength; i += storeInterval)
            anchors.Add(E * i);
        // ensure last room is an anchor
        var last = E * (mainPathLength - 1);
        if (anchors[anchors.Count - 1] != last) anchors.Add(last);

        // 2) Carve each anchor→next as two directional siblings
        var fullPath = new List<Vector2Int>();
        for (int seg = 0; seg < anchors.Count - 1; seg++)
        {
            var start = anchors[seg];
            var end = anchors[seg + 1];

            // Branch A only uses [NE, E]
            var pathA = CarveDirected(start, end, new[] { NE, E });

            // Branch B only uses [SE, E]
            var pathB = CarveDirected(start, end, new[] { SE, E });

            // Append both (skip B’s first so we don’t dup the anchor)
            fullPath.AddRange(pathA);
            fullPath.AddRange(pathB.GetRange(1, pathB.Count - 1));
        }

        // 3) Instantiate: stores at anchors (except start), else rooms
        foreach (var cell in fullPath)
        {
            bool isStore = anchors.Contains(cell) && cell != Vector2Int.zero;
            var prefab = isStore ? roomStorePrefab : roomPrefab;
            Instantiate(prefab, AxialToWorld(cell), Quaternion.identity, transform);
        }
    }

    List<Vector2Int> CarveDirected(
        Vector2Int start,
        Vector2Int end,
        Vector2Int[] allowedDirs
    )
    {
        var segment = new List<Vector2Int> { start };
        occupied.Add(start);
        var current = start;

        // march until you hit exactly `end`
        while (current != end)
        {
            // pick from only the allowed directions that don't collide
            var candidates = new List<Vector2Int>();
            foreach (var d in allowedDirs)
            {
                var nxt = current + d;
                if (!occupied.Contains(nxt))
                    candidates.Add(nxt);
            }

            if (candidates.Count == 0)
                break; // totally boxed in

            // prefer the one that gets you closer to the end anchor
            candidates.Sort((a, b) =>
                Vector2Int.Distance(a, end)
                    .CompareTo(Vector2Int.Distance(b, end))
            );

            var chosen = candidates[0];
            occupied.Add(chosen);
            segment.Add(chosen);
            current = chosen;
        }

        // snap to the anchor if something went wrong
        if (current != end)
        {
            occupied.Add(end);
            segment.Add(end);
        }

        return segment;
    }

    Vector3 AxialToWorld(Vector2Int hex)
    {
        float x = hexSize * Mathf.Sqrt(3f) * (hex.x + hex.y * 0.5f);
        float z = hexSize * (1.5f * hex.y);
        return new Vector3(x, 0f, z);
    }
}
