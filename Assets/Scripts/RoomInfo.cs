using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    [SerializeField] public List<string> doorDirection;
    [SerializeField] public List<bool> doorOccupation;
    public List<bool> trueOccupancy;
    public bool locked = true;
    public bool completed = false;
    [SerializeField] private List<Spawner> spawners;
    public List<GameObject> entities = new();
    private GenerateDungeon dungeon;
    public bool fighting = false;

    void Start()
    {
        trueOccupancy = new(doorOccupation);
        dungeon = transform.parent.gameObject.GetComponent<GenerateDungeon>();
    }

    void Update () {
        if (completed == false && fighting && entities.Count == 0) {
            completed = true;
            dungeon.UnlockRooms();
        }
    }

    public void Lock() {
        foreach (Spawner s in spawners) {
            s.SpawnEnemies();
        }
        dungeon.LockRoom(transform.gameObject);
        fighting = true;
    }
}
