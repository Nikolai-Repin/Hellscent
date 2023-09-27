using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    public Image healthbar;
    public float healthAmount = 100;

    
    
    void Start()
    
    
    {
        Debug.Log("start: " + healthbar);
    }

    // Update is called once per frame
    void Update()
    {
       if (healthAmount <= 0){

        Application.LoadLevel("Your Dead");
       } 
       if (Input.GetKeyDown(KeyCode.E)){

        TakeDamage(20);
       }
    }

    public void TakeDamage(float damage){
      
       healthAmount -= damage;
       
       try {
        Debug.Log("before");
   healthbar.fillAmount = healthAmount / 100;
   Debug.Log("after");
       }
       catch (NullReferenceException ex){
        Debug.Log("caught exception");
        Debug.Log(ex);
       }
        Debug.Log("healthAmount :: " + (healthAmount/100.0));
        
    }
    
}
