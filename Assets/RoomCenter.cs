using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    [SerializeField] public GameObject boss;
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "player") {
            boss.GetComponent<Boss>().Awaken();
        }
    }
}
