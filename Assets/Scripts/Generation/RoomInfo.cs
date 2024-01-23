using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    private GameManager dungeonManager;
    public bool fighting = false;
    public bool oneDoor = false;
    [SerializeField] private bool bossRoom = false;
    [SerializeField] private bool activateLastEnemyEvent;
    [SerializeField] private GameObject closedCollisions;
    [SerializeField] private GameObject openedCollisions;
    [SerializeField] private bool doColorAdjustment = true;
    
    private GameObject minimap;

    void Start()
    {
        trueOccupancy = new(doorOccupation);
        openedCollisions = GetChildGameObject(transform.gameObject, "Collisions");
        dungeon = transform.parent.gameObject.GetComponent<GenerateDungeon>();
        if (bossRoom) {
            dungeonManager = dungeon.dungeonManager;
            spawners[0].enemyPool = new() {spawners[0].enemyPool[dungeonManager.getFloor() - 1]};
        }
        if (doColorAdjustment) {
            AdjustColors();
        }
        minimap = GameObject.Find("Minimap");
    }

    void Update () {
        // Figures out if the room is active and how many enemies are lefta nd what to do in each case
        if (completed == false && fighting) {
            if (entities.Count == 0) {
                minimap.SetActive(true);
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

    private GameObject GetChildGameObject(GameObject fromGameObject, string withName)
    {
        // Finds the child of this object with a specified name
        var allKids = fromGameObject.GetComponentsInChildren<Transform>();
        foreach (Transform k in allKids) {
            if (k.name == withName) {
                return k.gameObject;
            }
        }
        return null;
    }

    private void AdjustColors() {
        // Asjusts the colors of the room based on the floor
        Tilemap floor = transform.gameObject.GetComponent<Tilemap>();
        Tilemap walls = openedCollisions.GetComponent<Tilemap>();
        floor.color = dungeon.floorColor;
        walls.color = dungeon.floorColor;
    }

    public IEnumerator Lock() {
        // Locks the room and spawns enemies
        if (locked) {
            minimap.SetActive(false);
            closedCollisions.SetActive(true);
            dungeon.LockRoom(transform.gameObject);
        }
        foreach (Spawner s in spawners) {
            yield return StartCoroutine(s.SpawnEnemies());
        }
        fighting = true;
    }

    public void RemoveEntity(Entity e) {
        // Removes an entity from the room
        entities.Remove(e.gameObject);
    }
}
