using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu; // Assign in inspector
    private bool isShowing = false;

    void Update() {
        if (Input.GetKeyDown("escape")) {
            isShowing = !isShowing;
            menu.SetActive(isShowing);
            PauseGame(isShowing);
        }
    }
    
    public void PauseGame(bool b) {
        if (b) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }

    
}
