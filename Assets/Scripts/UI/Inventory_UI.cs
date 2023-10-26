using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;

    public Player player;

    public List<Slot_UI> slots = new List<Slot_UI>();

    [SerializeField] private Canvas canvas;

    private Slot_UI draggedSlot;
    private Image draggedIcon;

    private void Awake() {
        canvas = FindObjectOfType<Canvas>();
    }

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

    /* public void Remove(int slotID) {
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(
            player.inventory.slots[slotID].itemName);

        if(itemToDrop != null) {
            player.DropItem(itemToDrop);
            player.inventory.Remove(slotID);
            Refresh();
        }
    } */

    // Drag and dropping items
    public void SlotBeginDrag(Slot_UI slot) {
        draggedSlot = slot;
        draggedIcon = Instantiate(draggedSlot.itemIcon);
        draggedIcon.transform.SetParent(canvas.transform);
        draggedIcon.raycastTarget = false;
        draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);

        MoveToMousePosition(draggedIcon.gameObject);
        Debug.Log("Start Drag : " + draggedSlot.name);
    }

    public void SlotDrag() {
        Debug.Log("Dragging: " + draggedSlot.name);
    }

    public void SlotEndDrag() {
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