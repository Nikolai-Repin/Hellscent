using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Keybind : MonoBehaviour {
    
    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI buttonLbl;
    
    private void Start () {
        buttonLbl.text = PlayerPrefs.GetString("CustomKey");
    }

    private void Update () {

        if (buttonLbl.text == "Awaiting Input") {

            foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode))) {

                if (Input.GetKey(keycode)) {

                    buttonLbl.text = keycode.ToString();
                    PlayerPrefs.SetString("CustomKey", keycode.ToString());
                    PlayerPrefs.Save();                
                }
            }
        }
    }

    public void ChangeKey() {
        buttonLbl.text = "Awaiting Input";
    }
}
