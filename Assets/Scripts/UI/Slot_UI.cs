using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot_UI : MonoBehaviour
{
    public int slotID;
    public Inventory inventory;

    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    public GameObject highlight;

    // Sets the slot to have that Item
    public void SetItem(Inventory.Slot slot) {
        if(slot != null) {
            itemIcon.sprite = slot.icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            quantityText.text = slot.count.ToString();
        }
    }

    // Sets the slot as empty
    public void SetEmpty() {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    // Turns on the highlight around the slot when slot is selected
    public void SetHighlight(bool isOn) {
        highlight.SetActive(isOn);
    }
}