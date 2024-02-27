using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Item : MonoBehaviour
{
    private GameObject playerCharacter;
    private PlayerController controller;
    private UIManager uiManager;
    
    [SerializeField] private List<ItemEffect> effects;
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
            
            foreach (ItemEffect i in effects) {
                i.Apply(controller);
                Debug.Log(i);
            }

            uiManager.updateHealth();

            if (journalPage.Length > 0 && pageManager.lastPage < 8) {
                pageManager.texts[pageManager.lastPage + 2] = journalPage[pageManager.lastPage];
                pageManager.lastPage++;
            }

            Destroy(gameObject);
        }
    }

    public int GetWeight() {
        return weight;
    }
}

[System.Serializable]
public class ItemEffect
{
    private enum ModifyType {
        none,
        additive,
        multiplicative,
        exponential,
    }

    private enum Stat {
        damage,
        fireDelay,
        chargeSpeed,
        maxMana,
        manaRechargeSpeed,
        HP,
        maxHP,
        moveSpeed,
        kickback,
    }

    [SerializeField] private ModifyType modType;
    [SerializeField] private Stat statType;
    [SerializeField] private float amount;

    public void Apply(PlayerController target) {
        switch (statType) {
            case Stat.damage: {
                Weapon targetWeapon = target.equippedWeapon.GetComponent<Weapon>();
                targetWeapon.Damage = ModifyStat(targetWeapon.Damage, amount, modType);
                break;
            }

            case Stat.fireDelay: {
                Weapon targetWeapon = target.equippedWeapon.GetComponent<Weapon>();
                targetWeapon.cooldownTime = ModifyStat(targetWeapon.cooldownTime, amount, modType);
                break;
            }

            case Stat.chargeSpeed: {
                Weapon targetWeapon = target.equippedWeapon.GetComponent<Weapon>();
                targetWeapon.chargeTime = ModifyStat(targetWeapon.chargeTime, amount, modType);
                break;
            }

            case Stat.maxMana: {
                target.AddMaxMana(amount);
                break;
            }

            case Stat.manaRechargeSpeed: {
                target.AddManaRechargeSpeed(amount);
                break;
            }

            case Stat.HP: {
                target.RestoreHP(amount);
                break;
            }

            case Stat.maxHP: {
                target.AddMaxHP(amount);
                break;
            }

            case Stat.moveSpeed: {
                target.AddSpeed(amount);
                break;
            }

            case Stat.kickback: {
                Weapon targetWeapon = target.equippedWeapon.GetComponent<Weapon>();
                targetWeapon.kickback = ModifyStat(targetWeapon.kickback, amount, modType);
                break;
            }
        }
    }

    // Method to modify a stat based on the given ModifyType
    private static float ModifyStat(float baseValue, float modifier, ModifyType modifyType)
    {
        switch (modifyType)
        {
            case ModifyType.none:
                return baseValue; // No modification
                break;
            case ModifyType.additive:
                return baseValue + modifier; // Additive modification
                break;
            case ModifyType.multiplicative:
                return baseValue * modifier; // Multiplicative modification
                break;
            case ModifyType.exponential:
                return Mathf.Pow(baseValue, modifier); // Exponential modification
                break;
            default:
                return baseValue;
                Debug.Log("Invalid ModifyType");
                break;
        }
    }
}