using System;
using UnityEngine;
using TMPro;

public class Keybind : MonoBehaviour {
    
    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI buttonAttack;
    [SerializeField] private TextMeshProUGUI buttonPickUp;
    [SerializeField] private TextMeshProUGUI buttonItem;
    
    private void Start () {
        if (PlayerPrefs.GetInt("Attack") == 0) {

            PlayerPrefs.SetInt("Attack", (int) KeyCode.Mouse0);
            buttonAttack.text = "Mouse0";
        } else {

            buttonAttack.text = PlayerPrefs.GetString("aText");
        } if (PlayerPrefs.GetInt("Grab") == 0) {
            PlayerPrefs.SetInt("Grab", (int) KeyCode.E);
            buttonPickUp.text = "E";
        } else {
            buttonPickUp.text = PlayerPrefs.GetString("gText");

        } if (PlayerPrefs.GetInt("Swap") == 0) {
            PlayerPrefs.SetInt("Swap", (int) KeyCode.R);
            buttonItem.text = "R";
        } else {
            buttonItem.text = PlayerPrefs.GetString("sText");
        }
    }

    private void Update () {
        ChangeKey(buttonAttack, "Attack", "aText");
        ChangeKey(buttonItem, "Grab", "gText");
        ChangeKey(buttonPickUp, "Swap", "sText");
        
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

    public void ChangeKey(TextMeshProUGUI button, String k, String t) {
        if (button.text == "Awaiting Input") {

            foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode))) {

                if (Input.GetKey(keycode)) {

                    button.text = keycode.ToString();
                    PlayerPrefs.SetInt(k, (int) keycode);
                    PlayerPrefs.SetString(t, keycode.ToString());
                    PlayerPrefs.Save();
                                    
                }
            }
        }
    }
}
