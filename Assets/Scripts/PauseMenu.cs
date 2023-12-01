using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu; // Assign in inspector
    public GameObject options;
    private bool isShowing = false;
    public float time;

    void Update() {
        if (Input.GetKeyDown("escape")) {
            isShowing = !isShowing;
            menu.SetActive(isShowing);
            PauseGame(isShowing);
            if (!isShowing) {
                options.SetActive(false);
            }
        }
        time = Time.time;
    }
    
    public void PauseGame(bool b) {
        if (b) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }

    public void Resume() {
        menu.SetActive(false);
        PauseGame(false);
    }

    
}