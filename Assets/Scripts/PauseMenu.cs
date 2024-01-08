using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu; // Assign in inspector
    public GameObject options;
    private bool isShowing = false;
    public float time;
    public int targetScene;

    void Update() {
        if (Input.GetKeyDown("tab")) {
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

    public void Quit() {
        SceneManager.LoadScene(targetScene);
        PauseGame(false);
    }

    
}
