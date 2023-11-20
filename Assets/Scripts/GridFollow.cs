using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GridFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private int cellSize;
    private AstarPath astar;
    private Pathfinding.AstarData data;
    private Pathfinding.GridGraph gg;

    void Start() {
        astar = AstarPath.active;

        Pathfinding.AstarData data = astar.astarData;

        gg = data.gridGraph;
    }

    void Update() {
        Vector3 original = gg.center;
        gg.center = player.position;
        int x = (int)gg.center.x / cellSize;
        int y = (int)gg.center.y / cellSize;
        gg.center = new Vector2(x, y) * cellSize;
        if (original != gg.center) {
            astar.Scan ();
        }
    }
}
