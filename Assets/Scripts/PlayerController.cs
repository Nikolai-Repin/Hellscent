using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float dash; // default should be 150
    [SerializeField, Range(0,1)] private float damper; // default should be 150
    private Vector2 direction;
    private Vector2 saved_direction;

    //Weapon Variables
    [SerializeField] private float manaRechargeSpeed = 5;
    private int weaponIndex;
    private double rHoldTime;
    private bool hasWeapon = false;
    public GameObject equippedWeapon;
    public List<GameObject> heldWeapons;
    [SerializeField] private float damage;

    public Animator anim;


    private float pickupDistance;
    private ContactFilter2D itemContactFilter;

    //private UIManager uiManager;

    private float invulnTime;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        weaponIndex = 0;
        damage = 0f;
        pickupDistance = 5;
        rHoldTime = Time.time;
        invulnTime = Time.time - 1;
        itemContactFilter = new ContactFilter2D();
        itemContactFilter.SetLayerMask(LayerMask.GetMask("Items"));
        anim = gameObject.GetComponent<Animator>();
        Register();
    }

    void Update()
    {
        direction = new Vector2(0.0f, 0.0f);
        bool keypressed = false;

        float controlx = Input.GetAxisRaw("Horizontal");
        float controly = Input.GetAxisRaw("Vertical");

        direction = new Vector2(controlx, controly);
        keypressed = controlx != 0 || controly != 0;
        direction = direction.normalized;

        if (keypressed) {
            saved_direction = direction;
        }

        // Makes the player dash. Dash scales with speed and dash variables.
        if (Input.GetKeyDown(KeyCode.Space)) {
             rb.velocity += saved_direction * ((dash*150*0.7f) + (speed*150*0.3f)) * Time.deltaTime;
        }

        if (Input.GetKeyDown((KeyCode) PlayerPrefs.GetInt("Grab"))) {
            Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, pickupDistance, LayerMask.GetMask("Items"));
            if (results.Length > 0) {
                PickupWeapon(FindClosest(results, transform.position));
            }
        }

        rb.velocity *= Mathf.Pow(1f - damper, Time.deltaTime * 10f);

        if (Input.GetKeyDown(KeyCode.D)){
            anim.Play("playerWalkRight");
        }
        if(Input.GetKeyDown(KeyCode.A)){
            anim.Play("playerWalkLeft");
        }

        if (hasWeapon) {
            if (Input.GetKeyDown((KeyCode) PlayerPrefs.GetInt("Swap"))) {
                rHoldTime = Time.time;
            }
            if (Input.GetKeyUp((KeyCode) PlayerPrefs.GetInt("Swap"))) {
                if ((Time.time - rHoldTime)<0.5) {
                    ChangeWeapon((weaponIndex+1)%(heldWeapons.Count));
                    //Drop held weapon if r was held for longer
                }

                else {
                    DropWeapon(weaponIndex);
                }
            }
            
            if (Input.GetKey((KeyCode) PlayerPrefs.GetInt("Attack"))) {
                if(equippedWeapon.GetComponent<Weapon>().Fire()) {

                    //Kickback from successful shot
                    Vector2 kbVector = new Vector2(Mathf.Cos(equippedWeapon.transform.rotation.eulerAngles.z*Mathf.Deg2Rad), Mathf.Sin(equippedWeapon.transform.rotation.eulerAngles.z*Mathf.Deg2Rad)).normalized;
                    kbVector *= equippedWeapon.GetComponent<Weapon>().kickback*-1;
                    rb.velocity += kbVector;
                }
            }
        }

        rb.velocity += direction * speed * Time.deltaTime; 
    }

    //Changes the currently held weapon to index i in heldWeapons
    public void ChangeWeapon(int i) {
        if (equippedWeapon != null) {
            equippedWeapon.GetComponent<SpriteRenderer>().enabled = false;
        }
        weaponIndex = i;
        equippedWeapon = heldWeapons[weaponIndex];
        equippedWeapon.GetComponent<SpriteRenderer>().enabled = true;
    }

    //Drops the weapon at index i in heldWeapons
    public void DropWeapon(int i) {
        //Handling what happens to the selected weapon if it's being dropped
        if (equippedWeapon == heldWeapons[i]) {
            if (heldWeapons.Count > 1) {
                ChangeWeapon((weaponIndex+1)%(heldWeapons.Count-1));
            } else {
                hasWeapon = false;
            }
        }
        
        //Creating the weapon as a dropped item
        GameObject DroppedItem = Resources.Load<GameObject>("Prefabs/DroppedItem"); //This line is bad, lmk if there's a better way to do this, p l e a s e
        GameObject droppedWeapon = Instantiate(DroppedItem, transform.position, new Quaternion());
        droppedWeapon.name = droppedWeapon.name.Replace("(Clone)","").Trim();
        droppedWeapon.GetComponent<PickupItem>().SetItem(heldWeapons[i]);

        //Removing the held weapon
        Destroy(heldWeapons[i]);
        heldWeapons.RemoveAt(i);
    }
    //Registers a new weapon in the player's list of held weapons
    public void NewWeapon(GameObject weapon) {
        hasWeapon = true;
        weapon.transform.SetParent(transform);
        weapon.GetComponent<Weapon>().GetControllerAndEquip();
        heldWeapons.Add(weapon);
        ChangeWeapon(heldWeapons.Count-1);
    }

    //Picks up target dropped item off the ground
    public void PickupWeapon(GameObject target) {

        //Add the weapon to the arsonal
        GameObject newWeapon = Instantiate(target.GetComponent<PickupItem>().GetItem(), transform.position, new Quaternion());
        newWeapon.transform.parent = gameObject.transform;
        newWeapon.transform.localScale = new Vector3(2, 2, 0);

        //Destroy the weapon on the ground
        target.GetComponent<PickupItem>().CleanUp();
    }

    //Deals damage to entity if vulnerable, returns true if damage was dealt
    public override bool TakeDamage(float damage) {
        if (!invulnerable && Time.time >= invulnTime) {
            healthAmount -= damage;
            uiManager.updateHealth();
            if (healthAmount <= 0) {
                Die();
            }
            invulnTime = Time.time + 1F;
            Debug.Log("Damaged");
            return true;
        }
        return (false);
   }

    //Overrides Die() in Entity so player isn't destroyed on death
    public override void Die () {
        return;
    }

    //Returns percentage of current mana out of maxMana
    public float GetManaPercent() {
        return equippedWeapon.GetComponent<Weapon>().GetManaPercent();
    }

    //Returns mana recharge speed
    public float GetManaRechargeSpeed() {
        return manaRechargeSpeed;
    }

    //Returns damage bonus
    public override float GetDamage() {
        return damage;
    }

    // Changes the damage that the player deals using a weapon.
    public void AddDamage(float bonusDamage) {
        damage += bonusDamage;
    }

    // Changes the player's speed
    public void AddSpeed(float bonusSpeed) {
        speed += bonusSpeed;
    }

    // Changes the Player's max mana
    public void AddMaxMana(float bonusMaxMana) {
        equippedWeapon.GetComponent<Weapon>().AddMaxMana(bonusMaxMana);
    }

    public void AddManaRechargeSpeed(float bonus) {
        manaRechargeSpeed += bonus;
    }

    public override void Reset() {
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;
    }

}