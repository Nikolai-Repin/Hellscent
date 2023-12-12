using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    [SerializeField] private bool defualtOn;

    void Update() {
        transform.gameObject.GetComponent<Image>().enabled = defualtOn ? !manager.hide : manager.hide;
    }
}
