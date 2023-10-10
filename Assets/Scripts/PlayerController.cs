using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float dash;
    [SerializeField, Range(0,1)] private float damper;

    [SerializeField] public Weapon equippedWeapon;
    public List<Weapon> heldWeapons;
    private int weaponIndex;
    private bool hasWeapon = false;

    private Vector2 direction;
    private Vector2 saved_direction;

    [SerializeField] private static float damage;



    void Start() {
        rb = GetComponent<Rigidbody2D>();
        //heldWeapons = new List<Weapon>();
        weaponIndex = 0;
        Debug.Log(heldWeapons.Count + "Player Side");
        damage = 20f;
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
        if (Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity += saved_direction * dash * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("Pickup Attempt");
        }

        rb.velocity *= Mathf.Pow(1f - damper, Time.deltaTime * 10f);

        if (hasWeapon) {
            if (Input.GetKeyDown(KeyCode.R)) {
                ChangeWeapon((weaponIndex+1)%(heldWeapons.Count));
            }
            
            if (Input.GetMouseButton(0)) {
                if(equippedWeapon.Fire()) {
                    Vector2 kbVector = new Vector2(Mathf.Cos(equippedWeapon.transform.rotation.eulerAngles.z*Mathf.Deg2Rad), Mathf.Sin(equippedWeapon.transform.rotation.eulerAngles.z*Mathf.Deg2Rad)).normalized;
                    kbVector *= equippedWeapon.GetComponent<Weapon>().kickback*-1;
                    rb.velocity += kbVector;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            Debug.Log(GetDamage());
        }

        rb.velocity += direction * speed * Time.deltaTime; 
    }

    public void ChangeWeapon(int i) {
        if (equippedWeapon != null) {equippedWeapon.transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;}
        weaponIndex = i;
        equippedWeapon = heldWeapons[weaponIndex];
        equippedWeapon.transform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void NewWeapon(Weapon w) {
        hasWeapon = true;
        heldWeapons.Add(w);
        ChangeWeapon(heldWeapons.Count-1);
    }

    public static float GetDamage() {
        return damage;
    }

    // Method to increase the damage that the player deals using a weapon.
    public static void AddDamage(float BonusDamage) {
        damage += BonusDamage;
    }
}
