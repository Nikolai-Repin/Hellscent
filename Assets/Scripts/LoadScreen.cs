using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    [SerializeField] private bool defaultOn;

    private Image image;

    void Start() {
        image = transform.gameObject.GetComponent<Image>();
    }

    void Update() {
        if (image.enabled) {
            image.enabled = defaultOn ? !manager.hide : manager.hide;
        }
    }
}
