using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private GameObject playerCharacter;
    public float bonusDamage;
    public int bonusAmmo;
    public float bonusSpeed;

    void Start() {
        playerCharacter = GameObject.FindWithTag("player");
    }

    // Triggers various item effects with if conditions when coming in contact with the player.
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("player")) {
            Debug.Log("touched an item!");
            
            if (bonusDamage > 0) {
                playerCharacter.GetComponent<Controller>().AddDamage(bonusDamage);
            }

            if (bonusAmmo > 0) {
                playerCharacter.GetComponent<Controller>().equippedWeapon.addAmmo(bonusAmmo);
            }

            if (bonusSpeed > 0) {
                playerCharacter.GetComponent<Controller>().AddSpeed(bonusSpeed);
            }

            Destroy(gameObject);
        }
    }

}
