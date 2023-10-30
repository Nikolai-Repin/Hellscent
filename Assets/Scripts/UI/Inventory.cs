using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public CollectableType type;
        public int count;
        public int maxAllowed;

        public Sprite icon;

        public Slot() {
            type = CollectableType.NONE;
            count = 0;
            maxAllowed = 10;
        }

        // Checks to see if can add item to that stack
        public bool CanAddItem() {
            if(count < maxAllowed) {
                return true;
            }
            return false;
        }

        public void AddItem(Collectable item) {
            this.type = item.type;
            this.icon = item.icon;
            count++;
        }

        public void RemoveItem() {
            if(count > 0) {
                count--;

                if(count == 0) {
                    icon = null;
                    type = CollectableType.NONE;
                }
            }
        }
    }

    public List <Slot> slots = new List<Slot>();

    // Goes through the inventory to find an empty slot
    public Inventory(int numSlots) {
        for(int i = 0; i < numSlots; i++) {
            Slot slot = new Slot();
            slots.Add(slot);
        }
    }
    public void Add(Collectable item) {
        foreach(Slot slot in slots) {
            if(slot.type == item.type && slot.CanAddItem()) {
                slot.AddItem(item);
                return;
            }
        }
        
        foreach(Slot slot in slots) {
            // If item matches the item in inventory, adds to that stack
            if(slot.type == CollectableType.NONE) {
                slot.AddItem(item);
                return;
            }
        }
    }

    public void Remove(int index) {
        slots[index].RemoveItem();  
    }
}
