using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
     public float healthAmount = 100;
        public Image healthbar;
    
    
    void Start()
    
    
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       if (healthAmount <= 0){

        Log.Break();
       } 
      
    }

    public void TakeDamage(float damage){
      
       healthAmount -= damage;
      

      healthbar.fillAmount = healthAmount / 100;

       }

        
    }
    

