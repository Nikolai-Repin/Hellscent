using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Inventory
    // -------------------------------------------------------------------------------------
    public InventoryManager inventory;

    private void Awake() {
        inventory = GetComponent<InventoryManager>();
    }

    // Item spawns in a random location around the player when dropped
    public void DropItem(Item item) {
        Vector2 spawnLocation = transform.position;

        Vector2 spawnOffset = Random.insideUnitCircle * 1.25f;

        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);

        droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop) {
        for(int i = 0; i < numToDrop; i++) {
            DropItem(item);
        }
    }
}