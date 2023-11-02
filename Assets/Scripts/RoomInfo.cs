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

    void Start()
    {
        trueOccupancy = new(doorOccupation);
        dungeon = transform.parent.gameObject.GetComponent<GenerateDungeon>();
    }

    public void Lock() {
        //dungeon.LockRoom(transform.gameObject);
        foreach (Spawner s in spawners) {
            s.SpawnEnemies();
        }
        completed = true;
    }
}
