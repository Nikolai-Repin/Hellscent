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

    public void Update() {
        Refresh();
    }

    public void Refresh() {
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
            inventory.slots[UI_Manager.draggedSlot.slotID].itemName);

        if(itemToDrop != null) {
            if(UI_Manager.dragSingle) {
                UIGameManager.instance.player.DropItem(itemToDrop);
                inventory.Remove(UI_Manager.draggedSlot.slotID);
            }

            else {
                UIGameManager.instance.player.DropItem(itemToDrop, inventory.slots[UI_Manager.draggedSlot.slotID].count);
                inventory.Remove(UI_Manager.draggedSlot.slotID, inventory.slots[UI_Manager.draggedSlot.slotID].count);
            }
            Refresh();
        }

        UI_Manager.draggedSlot = null;
    }

    // Drag and dropping items
    public void SlotBeginDrag(Slot_UI slot) {
        UI_Manager.draggedSlot = slot;

        UI_Manager.draggedIcon = Instantiate(slot.itemIcon);
        UI_Manager.draggedIcon.raycastTarget = false;
        UI_Manager.draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);
        UI_Manager.draggedIcon.transform.SetParent(canvas.transform);

        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
    }

    public void SlotDrag() {
        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
    }

    public void SlotEndDrag() {
        Destroy(UI_Manager.draggedIcon.gameObject);
        UI_Manager.draggedIcon = null;
    }

    public void SlotDrop(Slot_UI slot) {
        if(UI_Manager.dragSingle) {
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory);
        }

        else {
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory,
                UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID].count);
        }
        UIGameManager.instance.uiManager.RefreshAll();
        
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
