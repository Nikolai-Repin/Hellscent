using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{    
    public float bonusDamage;

    // Triggers Item effect (only increasing bullet damage for now) when it comes in contact with the player.
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("player")) {
            Debug.Log("touched an item!");
            
            if (bonusDamage > 0) {
                Controller.AddDamage(bonusDamage);
            }

            Destroy(gameObject);
        }
    }

}
