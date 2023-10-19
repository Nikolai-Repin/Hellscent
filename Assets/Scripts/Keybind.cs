using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Keybind : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI buttonLbl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonLbl.text == "Awaiting Input") {
            foreach (KeyCode keycode in Enum.GetValues(typeOf(KeyCode))) {
                if (Input.GetKey(keycode)) {
                    
                }
            }
        }
    }

    public void ChangeKey() {
        buttonLbl.text = "Awaiting Input";
    }
}
