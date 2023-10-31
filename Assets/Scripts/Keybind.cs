using System;
using UnityEngine;
using TMPro;

public class Keybind : MonoBehaviour {
    
    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI buttonAttack;
    [SerializeField] private TextMeshProUGUI buttonPickUp;
    [SerializeField] private TextMeshProUGUI buttonItem;
    
    private void Start () {
        buttonAttack.text = PlayerPrefs.GetString("CustomKey1");
        buttonPickUp.text = PlayerPrefs.GetString("CustomKey2");
        buttonItem.text = PlayerPrefs.GetString("CustomKey3");
    }

    private void Update () {
        ChangeKey(buttonAttack, "CustomKey1");
        ChangeKey(buttonPickUp, "CustomKey2");
        ChangeKey(buttonItem, "CustomKey3");
        
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
                    PlayerPrefs.SetString(k, keycode.ToString());
                    PlayerPrefs.Save();
                                    
                }
            }
        }
    }
}
