using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private GameObject playerCharacter;
    private Controller controller;
    private UIManager uiManager;


    [SerializeField] private float bonusDamage;
    [SerializeField] private int bonusMaxMana;
    [SerializeField] private float bonusSpeed;
    [SerializeField] private float bonusManaRechargeSpeed;
    [SerializeField] private float bonusMaxHP;

    void Start() {
        playerCharacter = GameObject.FindWithTag("player");
        controller = playerCharacter.GetComponent<Controller>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
    }

    // Triggers various item effects with if conditions when coming in contact with the player.
    // Every mention of controller is simply accessing the "controller" script within player, which just changes some values like damage and speed that the player has.
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("player")) {
            Debug.Log("touched an item!");
            
            if (bonusDamage > 0) {
                controller.AddDamage(bonusDamage);
            }

            if (bonusMaxMana > 0) {
                controller.AddMaxMana(bonusMaxMana);
            }

            if (bonusSpeed > 0) {
                controller.AddSpeed(bonusSpeed);
            }

            if (bonusManaRechargeSpeed > 0) {
                controller.AddManaRechargeSpeed(bonusManaRechargeSpeed);
            }

            if (bonusMaxHP > 0) {
                controller.AddMaxHP(bonusMaxHP);
                uiManager.updateHealth();
            }

            Destroy(gameObject);
        }
    }

}
