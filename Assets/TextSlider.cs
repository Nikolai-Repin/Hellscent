using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI numberText;

    void Start() {

    }

    void Update() {
        numberText.text = slider.value.ToSting();
    }
}
