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
    public bool oneDoor = false;
    [SerializeField] private bool activateLastEnemyEvent;
    [SerializeField] private GameObject closedCollisions;

    void Start()
    {
        trueOccupancy = new(doorOccupation);
        dungeon = transform.parent.gameObject.GetComponent<GenerateDungeon>();
    }

    void Update () {
        if (completed == false && fighting) {
            if (entities.Count == 0) {
                completed = true;
                dungeon.UnlockRooms();
                closedCollisions.SetActive(false);
            }   else if (activateLastEnemyEvent &&  entities.Count == 1) {
                entities[0].GetComponent<Entity>().LastEntityEvent();
                activateLastEnemyEvent = false;
                Debug.Log(entities);
            }
        } 
    }

    public IEnumerator Lock() {
        if (locked) {
            closedCollisions.SetActive(true);
            dungeon.LockRoom(transform.gameObject);
        }
        foreach (Spawner s in spawners) {
            yield return StartCoroutine(s.SpawnEnemies());
        }
        fighting = true;
    }

    public void RemoveEntity(Entity e) {
        entities.Remove(e.gameObject);
    }
}
