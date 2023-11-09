using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;

    public string inventoryName;

    public List<Slot_UI> slots = new List<Slot_UI>();

    [SerializeField] private Canvas canvas;

    private Slot_UI draggedSlot;
    private Image draggedIcon;
    private bool dragSingle;

    private Inventory inventory;

    private void Awake() {
        canvas = transform.root.GetComponent<Canvas>();
    }

    private void Start() {
        inventory = UIGameManager.instance.player.inventory.GetInventoryByName(inventoryName);

        SetupSlots();
        Refresh();
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
        // Only opens up inventory and not toolbar
        if(inventoryPanel != null) {
            // Checks if Inventory is opened or not
            if(!inventoryPanel.activeSelf) {
                inventoryPanel.SetActive(true);
                Refresh();
            }
            else {
                inventoryPanel.SetActive(false);
            }
        }
    }

    void Refresh() {
        // Loops through all the slots in the inventory
        if(slots.Count == inventory.slots.Count) {
            for(int i = 0; i < slots.Count; i++) {
                // Checks to see if there is an item in the slot
                if(inventory.slots[i].itemName != "") {
                    slots[i].SetItem(inventory.slots[i]);
                }

                else {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    // Remove item from inventory
    public void Remove() {
        Item itemToDrop = UIGameManager.instance.itemManager.GetItemByName(
            inventory.slots[draggedSlot.slotID].itemName);

        if(itemToDrop != null) {
            if(dragSingle) {
                UIGameManager.instance.player.DropItem(itemToDrop);
                inventory.Remove(draggedSlot.slotID);
            }

            else {
                UIGameManager.instance.player.DropItem(itemToDrop, inventory.slots[draggedSlot.slotID].count);
                inventory.Remove(draggedSlot.slotID, inventory.slots[draggedSlot.slotID].count);
            }
            Refresh();
        }

        draggedSlot = null;
    }

    // Drag and dropping items
    public void SlotBeginDrag(Slot_UI slot) {
        draggedSlot = slot;

        draggedIcon = Instantiate(draggedSlot.itemIcon);
        draggedIcon.raycastTarget = false;
        draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);
        draggedIcon.transform.SetParent(canvas.transform);

        MoveToMousePosition(draggedIcon.gameObject);
    }

    public void SlotDrag() {
        MoveToMousePosition(draggedIcon.gameObject);
    }

    public void SlotEndDrag() {
        Destroy(draggedIcon.gameObject);
        draggedIcon = null;
    }

    public void SlotDrop(Slot_UI slot) {
        draggedSlot.inventory.MoveSlot(draggedSlot.slotID, slot.slotID, slot.inventory);
        Refresh();
    }

    // Item follows mouse cursor while dragging
    private void MoveToMousePosition(GameObject toMove) {
        if(canvas != null) {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, null, out position);

            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    void SetupSlots() {
        int counter = 0;

        foreach(Slot_UI slot in slots) {
            slot.slotID = counter;
            counter++;
            slot.inventory = inventory;
        }
    }
}