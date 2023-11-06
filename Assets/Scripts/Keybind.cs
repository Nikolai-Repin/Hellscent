using System;
using UnityEngine;
using TMPro;

public class Keybind : MonoBehaviour {
    
    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI buttonAttack;
    [SerializeField] private TextMeshProUGUI buttonPickUp;
    [SerializeField] private TextMeshProUGUI buttonItem;
    
    private void Start () {
        buttonAttack.text = PlayerPrefs.GetString("Attack");
        buttonPickUp.text = PlayerPrefs.GetString("Grab");
        buttonItem.text = PlayerPrefs.GetString("Item");
    }

    private void Update () {
        ChangeKey(buttonAttack, "Attack");
        ChangeKey(buttonPickUp, "Swap");
        ChangeKey(buttonItem, "Grab");
        
    }

    public void ChangeAttackKey() {
        buttonAttack.text = "Awaiting Input";
    }

    public void ChangePickUpKey() {
        buttonPickUp.text = "Awaiting Input";
    }

    public void ChangeItemKey() {
        buttonItem.text = "Awaiting Input";
    }

    public void ChangeKey(TextMeshProUGUI button, String k) {
        if (button.text == "Awaiting Input") {

            foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode))) {

                if (Input.GetKey(keycode)) {

                    button.text = keycode.ToString();
                    PlayerPrefs.SetInt(k, (int) (keycode));
                    PlayerPrefs.Save();
                                    
                }
            }
        }
    }
}
