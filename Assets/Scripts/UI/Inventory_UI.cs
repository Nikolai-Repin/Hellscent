using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Player player;

    public List<Slot_UI> slots = new List<Slot_UI>();

    void Update() {
        // Press TAB to open Inventory
        if(Input.GetKeyDown(KeyCode.Tab)) {
            ToggleInventory();
        }
    }

    public void ToggleInventory() {
        // Checks if Inventory is opened or not
        if(!inventoryPanel.activeSelf) {
            inventoryPanel.SetActive(true);
            Setup();
        }
        else {
            inventoryPanel.SetActive(false);
        }
    }

    void Setup() {
        // Loops through all the slots in the inventory
        if(slots.Count == player.inventory.slots.Count) {
            for(int i = 0; i < slots.Count; i++) {
                // Checks to see if there is an item in the slot
                if(player.inventory.slots[i].type != CollectableType.NONE) {
                    slots[i].SetItem(player.inventory.slots[i]);
                }

                else {
                    slots[i].SetEmpty();
                }
            }
        }
    } 
}
