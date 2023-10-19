using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] protected bool vulnerable;
    [SerializeField] protected float healthAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Deals damage to entity if vulnerable, returns true if damage was dealt
    public bool TakeDamage(float damage) {
        if (vulnerable) {
            healthAmount -= damage;
            Destroy(transform);
            Debug.Log("Damaged");
            return true;
        }
        return (false);
   }
}
