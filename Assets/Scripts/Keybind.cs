using System;
using UnityEngine;
using TMPro;

public class Keybind : MonoBehaviour {
    
    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI buttonAttack;
    [SerializeField] private TextMeshProUGUI buttonGrab;
    [SerializeField] private TextMeshProUGUI buttonSwap;
    String defaultA = "Mouse0";
    String defaultG = "E";
    String defaultS = "R";
    
    private void Start () {
        if (PlayerPrefs.GetInt("Attack") == 0) {

            PlayerPrefs.SetInt("Attack", (int) KeyCode.Mouse0);
            buttonAttack.text = defaultA;
        } else {

            buttonAttack.text = PlayerPrefs.GetString("aText");
        } if (PlayerPrefs.GetInt("Grab") == 0) {

            PlayerPrefs.SetInt("Grab", (int) KeyCode.E);
            buttonGrab.text = defaultG;
        } else {

            buttonGrab.text = PlayerPrefs.GetString("gText");
        } if (PlayerPrefs.GetInt("swap") == 0) {
            
            PlayerPrefs.SetInt("Swap", (int) KeyCode.R);
            buttonSwap.text = defaultS;
        } else {

            buttonSwap.text = PlayerPrefs.GetString("sText");
        }
    }

    private void Update () {
        ChangeKey(buttonAttack, "Attack", "aText");
        
        ChangeKey(buttonGrab, "Grab", "gText");
        
        ChangeKey(buttonSwap, "Swap", "sText");
        
    }

    public void ChangeAttackKey() {
        if (buttonGrab.text != "Awaiting Input" && buttonSwap.text != "Awaiting Input") {
            buttonAttack.text = "Awaiting Input";
        }
    }

    public void ChangeGrabKey() {
        if (buttonAttack.text != "Awaiting Input" && buttonSwap.text != "Awaiting Input") {
            buttonGrab.text = "Awaiting Input";
        }
    }

    public void ChangeSwapKey() {
        if (buttonGrab.text != "Awaiting Input" && buttonAttack.text != "Awaiting Input") {
            buttonSwap.text = "Awaiting Input";
        }
    }


    public void ChangeKey(TextMeshProUGUI button, String k, String t) {
        if (button.text == "Awaiting Input") {

            foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode))) {

                if (Input.GetKey(keycode)) {
                    if ((int) keycode == PlayerPrefs.GetInt("Attack") || (int) keycode == PlayerPrefs.GetInt("Grab") || (int) keycode == PlayerPrefs.GetInt("Swap")) {
                        if (k.Equals("Attack")) {
                            button.text = defaultA;
                        } else if (k.Equals("Grab")) {
                            button.text = defaultG;
                        } else if (k.Equals("Swap")) {
                            button.text = defaultS;
                        }
                        return;
                    }
                    button.text = keycode.ToString();
                    PlayerPrefs.SetInt(k, (int) keycode);
                    PlayerPrefs.SetString(t, keycode.ToString());
                    PlayerPrefs.Save();
                                    
                }
                
            }
        }
    }
}
