using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image ManaBar;


    // Start is called before the first frame update
    void Start()
    {
        ManaBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
