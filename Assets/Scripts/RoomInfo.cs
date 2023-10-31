using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    [SerializeField] public List<string> doorDirection;
    [SerializeField] public List<bool> doorOccupation;
    public List<bool> trueOccupancy;
    [SerializeField] private bool locked = true;
    [SerializeField] private List<Spawner> spawners;
    public List<GameObject> entities = new();

    void Start()
    {
        trueOccupancy = new(doorOccupation);
    }

    void Update() {

    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "player") {
            Debug.Log("Player enters");
        }
    }
}
