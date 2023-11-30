using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryManager inventory;

    private void Awake() {
        inventory = GetComponent<InventoryManager>();
    }

    public void DropItem(UI_Items item) {
        Vector2 spawnLocation = transform.position;

        Vector2 spawnOffset = Random.insideUnitCircle * 1.25f;

        UI_Items droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);

        droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
    }

    public void DropItem(UI_Items item, int numToDrop) {
        for(int i = 0; i < numToDrop; i++) {
            DropItem(item);
        }
    }
}
