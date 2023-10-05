using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{    
    public double bonusDamage;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {        
    }

    // Triggers Item effect (only increasing bullet damage for now) when it comes in contact with the player.
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("touched an item!");
            
            if (bonusDamage > 0) {
                Controller.AddDamage(bonusDamage);
            }

            Destroy(gameObject);
        }
    }

}
