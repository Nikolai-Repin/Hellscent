using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI sliderText;

    void Start() {

    }

    void Update() {
        //sets volume slider value to master volume
        sliderText.text = slider.value.ToString();
    }
}
