using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup uiGroup;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    public void ShowUI() {
        fadeIn = false;
    }

    public void HideUi() {
        fadeOut = false;
    }

    private void Update() {
        if (fadeIn) {
            if (uiGroup.alpha < 1) {
                uiGroup.alpha += Time.deltaTime;
                if (uiGroup.alpha >= 1) {
                    fadeIn = false; 
                }
            }
        }
    }
}
