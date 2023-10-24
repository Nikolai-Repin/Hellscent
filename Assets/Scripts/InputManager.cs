using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    
    public static InputManager instance;

    public Keybind keybind;

    void Awake() {

        if (instance == null) {
            instance = this;

        } else if (instance != this) {
            Destroy(this);

        }
        DontDestroyOnLoad(this);
    }

    public bool GetKeyDown(string Key) {

        if (Input.GetKeyDown(keybind.CheckKey(key))) {
            return true;

        } else {
            return false;
            
        }
    }

    public bool GetKey(string key) {
        if (Input.GetKey(keybind.CheckKey(key))) {
            return true;

        } else {
            return false;
        }
    }
}
