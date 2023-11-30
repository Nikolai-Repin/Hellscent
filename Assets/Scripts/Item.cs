using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private GameObject playerCharacter;
    private PlayerController controller;
    private UIManager uiManager;
    
    public ItemData data;

    [SerializeField] private float bonusDamage;
    [SerializeField] private int bonusMaxMana;
    [SerializeField] private float bonusSpeed;
    [SerializeField] private float bonusManaRechargeSpeed;
    [SerializeField] private float bonusMaxHP;
    [SerializeField] private int weight;

    [HideInInspector] public Rigidbody2D rb2d;

    void Start() {
        playerCharacter = GameObject.FindWithTag("Player");
        controller = playerCharacter.GetComponent<PlayerController>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();

        rb2d = GetComponent<Rigidbody2D>();
    }

    // Triggers various item effects with if conditions when coming in contact with the player.
    // Every mention of controller is simply accessing the "controller" script within player, which just changes some values like damage and speed that the player has.
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            
            if (bonusDamage > 0) {
                controller.equippedWeapon.GetComponent<Weapon>().AddDamage(bonusDamage);
                Debug.Log("Damage increased by " + bonusDamage);
            }

            if (bonusMaxMana > 0) {
                controller.AddMaxMana(bonusMaxMana);
                Debug.Log("Max Mana increased by " + bonusMaxMana);
            }

            if (bonusSpeed > 0) {
                controller.AddSpeed(bonusSpeed);
                Debug.Log("Speed increased by " + bonusSpeed);
            }

            if (bonusManaRechargeSpeed > 0) {
                controller.AddManaRechargeSpeed(bonusManaRechargeSpeed);
                Debug.Log("Mana recovery increased by " + bonusManaRechargeSpeed);
            }

            if (bonusMaxHP > 0) {
                controller.AddMaxHP(bonusMaxHP);
                controller.RestoreHP(bonusMaxHP);
                uiManager.updateHealth();
                Debug.Log("Max Hp increased by " + bonusMaxHP);
                Debug.Log("Healed " + bonusMaxHP + " HP");
            }

            Destroy(gameObject);
        }
    }

    public int GetWeight() {
        return weight;
    }
}
