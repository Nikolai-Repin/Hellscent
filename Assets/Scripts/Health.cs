using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
   public float healthAmount = 100f;
   public Image healthbar;
   
   void Update()
   {
      if (healthAmount <= 0) {
         //Debug.Break();
      } 
   }

   public void TakeDamage(float damage) {
      healthAmount -= damage;

      healthbar.fillAmount = healthAmount / 100;
   }
}