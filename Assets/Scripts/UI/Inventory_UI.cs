using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;

    public Player player;

    public List<Slot_UI> slots = new List<Slot_UI>();

    private Slot_UI draggedSlot;
    private Image draggedIcon;
    private Canvas canvas;
    private bool dragSingle;

    private void Awake() {
        canvas = FindObjectOfType<Canvas>();
    }

    void Update() {
        // Press TAB to open Inventory
        if(Input.GetKeyDown(KeyCode.Tab)) {
            ToggleInventory();
        }

        // Hold LeftShift to drop 1 single item
        if(Input.GetKey(KeyCode.LeftShift)) {
            dragSingle = true;
        }
        else {
            dragSingle = false;
        }
    }

    public void ToggleInventory() {
        // Checks if Inventory is opened or not
        if(!inventoryPanel.activeSelf) {
            inventoryPanel.SetActive(true);
            Refresh();
        }
        else {
            inventoryPanel.SetActive(false);
        }
    }

    void Refresh() {
        // Loops through all the slots in the inventory
        if(slots.Count == player.inventory.slots.Count) {
            for(int i = 0; i < slots.Count; i++) {
                // Checks to see if there is an item in the slot
                if(player.inventory.slots[i].itemName != "") {
                    slots[i].SetItem(player.inventory.slots[i]);
                }

                else {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    // Remove item from inventory
    public void Remove() {
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(
            player.inventory.slots[draggedSlot.slotID].itemName);

        if(itemToDrop != null) {
            if(dragSingle) {
                player.DropItem(itemToDrop);
                player.inventory.Remove(draggedSlot.slotID);
            }

            else {
                player.DropItem(itemToDrop, player.inventory.slots[draggedSlot.slotID].count);
                player.inventory.Remove(draggedSlot.slotID, player.inventory.slots[draggedSlot.slotID].count);
            }
            Refresh();
        }

        draggedSlot = null;
    }

    // Drag and dropping items (Does not work)
    public void SlotBeginDrag(Slot_UI slot) {
        draggedSlot = slot;

        draggedIcon = Instantiate(slot.itemIcon);
        draggedIcon.raycastTarget = false;
        draggedIcon.rectTransform.sizeDelta = new Vector2(50f, 50f);
        draggedIcon.transform.SetParent(canvas.transform);

        MoveToMousePosition(draggedIcon.gameObject);

        Debug.Log("Begin Drag : " + slot.name);
    }

    public void SlotDrag() {
        MoveToMousePosition(draggedIcon.gameObject);
        Debug.Log("Dragging: " + draggedSlot.name);
    }

    public void SlotEndDrag() {
        Destroy(draggedIcon.gameObject);
        draggedIcon = null;

        Debug.Log("Done Dragging: " + draggedSlot.name);
    }

    public void SlotDrop(Slot_UI slot) {
        Debug.Log("Dropped " + draggedSlot.name + " on " + slot.name);
    }

    private void MoveToMousePosition(GameObject toMove) {
        if(canvas != null) {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, null, out position);

            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }
}