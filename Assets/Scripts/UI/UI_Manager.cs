using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();

    public List<Inventory_UI> inventoryUIs;

    public Slot_UI draggedSlot;
    public Image draggedIcon;

    private void Awake() {
        Inititalize();
    }

    public Inventory_UI GetInventoryUI(string inventoryName) {
        if(inventoryUIByName.ContainsKey(inventoryName)) {
            return inventoryUIByName[inventoryName];
        }
        Debug.LogWarning("There is not inventory UI for " + inventoryName);
        return null;
    }

    void Initialize() {
        foreach(Inventory_UI ui in inventoryUIs) {
            if(!inventoryUIByName.ContainsKey(ui.inventoryName)) {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }
}
