using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class Item : MonoBehaviour
{
    private GameObject playerCharacter;
    public float bonusDamage;
    public int bonusMaxMana;
    public float bonusSpeed;
    public float bonusManaRechargeSpeed;

    public ItemData data;
    [HideInInspector] public Rigidbody2D rb2d;

    void Start() {
        playerCharacter = GameObject.FindWithTag("player");
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Triggers various item effects with if conditions when coming in contact with the player.
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("player")) {
            Debug.Log("touched an item!");
            
            if (bonusDamage > 0) {
                playerCharacter.GetComponent<PlayerController>().AddDamage(bonusDamage);
            }

            if (bonusMaxMana > 0) {
                playerCharacter.GetComponent<PlayerController>().AddMaxMana(bonusMaxMana);
            }

            if (bonusSpeed > 0) {
                playerCharacter.GetComponent<PlayerController>().AddSpeed(bonusSpeed);
            }

            if (bonusManaRechargeSpeed > 0) {
                playerCharacter.GetComponent<PlayerController>().AddManaRechargeSpeed(bonusManaRechargeSpeed);
            }

            Destroy(gameObject);
        }
    }

/* We have an issue with two classes having the same name but different purposes, have Gabe and Dom resolve these


[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    

    private void Awake() {
        
    }
*/
}
