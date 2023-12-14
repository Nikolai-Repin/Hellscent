using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawn : MonoBehaviour
{
    void Start() {

    }

    public IEnumerator Spawn(GameObject entity, float spawnTime, RoomInfo room, GameObject tar) {
        yield return new WaitForSeconds(spawnTime);
        GameObject spawned = Instantiate(entity, tar.transform.position, Quaternion.Euler(0, 0, 0));
        spawned.GetComponent<Entity>().SetRoom(room);
        room.entities.Add(spawned);
        room.entities.Remove(tar);
        Destroy(tar);
    }
}
