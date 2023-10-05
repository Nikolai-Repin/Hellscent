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
    private Vector2 direction;
    private Vector2 saved_direction;

    private int weaponIndex;
    public List<Weapon> heldWeapons;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        heldWeapons = new List<Weapon>();
        weaponIndex = 0;
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

        if (Input.GetKeyDown(KeyCode.R)) {
            ChangeWeapon((weaponIndex+1)%(heldWeapons.Count));
        }

        rb.velocity *= Mathf.Pow(1f - damper, Time.deltaTime * 10f);
        if (Input.GetMouseButton(0)) {
            if(equippedWeapon.Fire()) {
                Vector2 kbVector = new Vector2(Mathf.Cos(equippedWeapon.transform.rotation.eulerAngles.z*Mathf.Deg2Rad), Mathf.Sin(equippedWeapon.transform.rotation.eulerAngles.z*Mathf.Deg2Rad)).normalized;
                kbVector *= equippedWeapon.GetComponent<Weapon>().kickback*-1;
                rb.velocity += kbVector;
            };
        }

        rb.velocity += direction * speed * Time.deltaTime; 
    }

    public void ChangeWeapon(int i) {
        if (equippedWeapon != null) {equippedWeapon.transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;}
        weaponIndex = i;
        equippedWeapon = heldWeapons[weaponIndex];
        equippedWeapon.transform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }


}
