using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();

    public GameObject inventoryPanel;

    public List<Inventory_UI> inventoryUIs;

    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;

    private void Awake() {
        Initialize();
    }

    void Initialize() {
        foreach(Inventory_UI ui in inventoryUIs) {
            if(!inventoryUIByName.ContainsKey(ui.inventoryName)) {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }

    private void Update() {
        // Press B to open and close inventory
        if(Input.GetKeyDown(KeyCode.B)) {
            ToggleInventoryUI();
        }

        // Hold LSHIFT to only drop 1 item
        if(Input.GetKey(KeyCode.LeftShift)) {
            dragSingle = true;
        }

        else {
            dragSingle = false;
        }
    }

    public void ToggleInventoryUI() {
        // Only opens up inventory and not toolbar
        if(inventoryPanel != null) {
            // Checks if Inventory is opened or not
            if(!inventoryPanel.activeSelf) {
                inventoryPanel.SetActive(true);
                RefreshInventoryUI("Backpack");
            }
            else {
                inventoryPanel.SetActive(false);
            }
        }
    }

    public void RefreshInventoryUI(string inventoryName) {
        if(inventoryUIByName.ContainsKey(inventoryName)) {
            inventoryUIByName[inventoryName].Refresh();
        }
    }

    public void RefreshAll() {
        foreach(KeyValuePair<string, Inventory_UI> keyValuePair in inventoryUIByName) {
            keyValuePair.Value.Refresh();
        }
    }

    public Inventory_UI GetInventoryUI(string inventoryName) {
        if(inventoryUIByName.ContainsKey(inventoryName)) {
            return inventoryUIByName[inventoryName];
        }
        Debug.LogWarning("There is not inventory UI for " + inventoryName);
        return null;
    }
}