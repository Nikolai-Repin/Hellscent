using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private GameObject playerCharacter;
    private PlayerController controller;
    private UIManager uiManager;

    [SerializeField] private float bonusDamage;
    [SerializeField] private int bonusMaxMana;
    [SerializeField] private float bonusSpeed;
    [SerializeField] private float bonusManaRechargeSpeed;
    [SerializeField] private float bonusMaxHP;
    [SerializeField] private float healHP;
    [SerializeField] private int weight;
    [SerializeField] private GameObject[] journalPage;
    [SerializeField] private Journalnavigation pageManager;

    public ItemData data;
    [HideInInspector] public Rigidbody2D rb2d;

    void Start() {
        pageManager = GameObject.Find("Main Camera").GetComponent<Journalnavigation>();
        playerCharacter = GameObject.FindWithTag("player");
        controller = playerCharacter.GetComponent<PlayerController>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();

        rb2d = GetComponent<Rigidbody2D>();
    }

    // Triggers various item effects with if conditions when coming in contact with the player.
    // Every mention of controller is simply accessing the "controller" script within player, which just changes some values like damage and speed that the player has.
    void OnTriggerStay2D(Collider2D other) {
        //Debug.Log("hello everyone]");
        if (other.CompareTag("player") && Input.GetKey((KeyCode) PlayerPrefs.GetInt("Grab")) ) {
            
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

            if (healHP > 0) {
                controller.RestoreHP(healHP);
                uiManager.updateHealth();
                Debug.Log("Healed " + healHP + " HP");
            }

            if (journalPage.Length > 0 && pageManager.lastPage < 8) {
                pageManager.texts[pageManager.lastPage + 4] = journalPage[pageManager.lastPage];
                pageManager.lastPage++;
            }

            Destroy(gameObject);
        }
    }

    public int GetWeight() {
        return weight;
    }
}
