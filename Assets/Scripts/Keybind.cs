using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybind", menuName = "Keybind")]
public class Keybind : ScriptableObject {
    
    public KeyCode pickup, switchW, fireW;

    public KeyCode CheckKey(string key) {
        
        switch (key) {
            case "Pickup":
                return pickup;

            case "Switch":
                return switchW;

            case "Fire":
                return fireW;

            default:
                return KeyCode.None;
        }
    }
}
