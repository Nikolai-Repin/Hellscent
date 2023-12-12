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
        if (!room.fighting && !room.completed && other.tag == "player") {
            StartCoroutine(room.Lock());
        }
    }
}
