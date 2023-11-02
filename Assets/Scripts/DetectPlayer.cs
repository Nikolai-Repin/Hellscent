using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private RoomInfo room;

    void Start()
    {
        room = transform.parent.gameObject.GetComponent<RoomInfo>();    
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (room.completed == false && room.locked == true && other.tag == "player") {
            other.transform.position = transform.GetChild(0).position;
            room.Lock();
        }
    }
}
