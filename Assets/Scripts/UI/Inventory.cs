using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public string itemName;
        public int count;
        public int maxAllowed;
        public string description;

        public Sprite icon;

        public Slot() {
            icon = null;
            itemName = "";
            description = "";
            count = 0;
            maxAllowed = 10;
        }

        public bool isEmpty {
            get {
                if(itemName == "" && count == 0) {
                    return true;
                }
                return false;
            }
        }

        // Checks to see if can add item to that stack
        public bool CanAddItem(string itemName) {
            if(this.itemName == itemName && count < maxAllowed) {
                return true;
            }
            return false;
        }

        // Adds the items to that slot
        public void AddItem(Item item) {
            this.itemName = item.data.itemName;
            this.icon = item.data.icon;
            this.description = item.data.description;
            count++;
        }

        public void AddItem(string itemName, Sprite icon, int maxAllowed, string description) {
            this.itemName = itemName;
            this.icon = icon;
            this.description = description;
            count++;
            this.maxAllowed = maxAllowed;
        }

        // Removes items from slots
        public void RemoveItem() {
            if(count > 0) {
                count--;

                if(count == 0) {
                    icon = null;
                    itemName = "";
                    description = "";
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
    public void Add(Item item) {
        foreach(Slot slot in slots) {
            if(slot.itemName == item.data.itemName && slot.CanAddItem(item.data.itemName)) {
                slot.AddItem(item);
                return;
            }
        }
        
        foreach(Slot slot in slots) {
            // If item matches the item in inventory, adds to that stack
            if(slot.itemName == "") {
                slot.AddItem(item);
                return;
            }
        }
    }

    public void Remove(int index) {
        slots[index].RemoveItem();  
    }

    public void Remove(int index, int numToRemove) {
        if(slots[index].count >= numToRemove) {
            for(int i = 0; i < numToRemove; i++) {
                Remove(index);
            }
        }
    }

    // Move items from slot to slot
    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove = 1) {
        Slot fromSlot = slots[fromIndex];
        Slot toSlot = toInventory.slots[toIndex];

        if(toSlot.isEmpty || toSlot.CanAddItem(fromSlot.itemName)) {
            for(int i = 0; i < numToMove; i++) {
                toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.maxAllowed, fromSlot.description);
                fromSlot.RemoveItem();
            }
        }
    }
}