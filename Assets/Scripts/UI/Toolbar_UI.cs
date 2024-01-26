using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbar_UI : MonoBehaviour
{
    [SerializeField] private List<Slot_UI> toolbarSlots = new List<Slot_UI>();

    private Slot_UI selectedSlot;

    private void Start() {
        SelectSlot(0);
    }

    private void Update() {
        CheckAlphaNumericKeys();
    }

    public void SelectSlot(int index) {
        if(toolbarSlots.Count == 2) {
            if(selectedSlot != null) {
                selectedSlot.SetHighlight(false);
            }
            selectedSlot = toolbarSlots[index];
            selectedSlot.SetHighlight(true);
        }
    }
    // Switch from first slot to second slot
    private void CheckAlphaNumericKeys() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            SelectSlot(0);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectSlot(1);
        }
    }
}