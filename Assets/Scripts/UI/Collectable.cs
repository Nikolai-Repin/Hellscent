using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
// Add this script to  anything that you want to become an Item, make sure the Item has a RigidBody 2D
public class Collectable : MonoBehaviour
{   
    // When player collides with a "collectable," they pick it up and adds it to Inventory
    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();

        if(player) {
            Item item = GetComponent<Item>();

            // If Item is not null, it gets added into the "Backpack" which is the Inventory
            if(item != null) {
                player.inventory.Add("Backpack", item);
                Destroy(this.gameObject);
            }
        }
    }
}