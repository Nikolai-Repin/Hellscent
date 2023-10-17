using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float dash;
    [SerializeField, Range(0,1)] private float damper;
    private Vector2 direction;
    private Vector2 saved_direction;

    //Weapon Variables
    private int weaponIndex;
    private double rHoldTime;
    private bool hasWeapon = false;
    private GameObject equippedWeapon;
    public List<GameObject> heldWeapons;

    private float pickupDistance;
    private ContactFilter2D itemContactFilter;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        weaponIndex = 0;
        pickupDistance = 10;
        rHoldTime = Time.time;
        itemContactFilter = new ContactFilter2D();
        itemContactFilter.SetLayerMask(LayerMask.GetMask("Items"));
    }

    // Update is called once per frame
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
            Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, pickupDistance, LayerMask.GetMask("Items"));
            PickupWeapon(results[0].transform.gameObject); //This line causes error
        }

        rb.velocity *= Mathf.Pow(1f - damper, Time.deltaTime * 10f);

        if (hasWeapon) {
            if (Input.GetKeyDown(KeyCode.R)) {
                rHoldTime = Time.time;
            }
            if (Input.GetKeyUp(KeyCode.R)) {
                if ((Time.time - rHoldTime)<1) {
                    ChangeWeapon((weaponIndex+1)%(heldWeapons.Count));
                } else {
                    DropWeapon(weaponIndex);
                }
            }
            
            if (Input.GetMouseButton(0)) {
                if(equippedWeapon.GetComponent<Weapon>().Fire()) {
                    Vector2 kbVector = new Vector2(Mathf.Cos(equippedWeapon.transform.rotation.eulerAngles.z*Mathf.Deg2Rad), Mathf.Sin(equippedWeapon.transform.rotation.eulerAngles.z*Mathf.Deg2Rad)).normalized;
                    kbVector *= equippedWeapon.GetComponent<Weapon>().kickback*-1;
                    rb.velocity += kbVector;
                }
            }
        }

        rb.velocity += direction * speed * Time.deltaTime; 
    }

    public void ChangeWeapon(int i) {
        if (equippedWeapon != null) {
            equippedWeapon.GetComponent<SpriteRenderer>().enabled = false;
        }
        weaponIndex = i;
        equippedWeapon = heldWeapons[weaponIndex];
        equippedWeapon.GetComponent<SpriteRenderer>().enabled = true;
    }

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
        //droppedWeapon.layer = LayerMask.NameToLayer("Items");

        //Removing the held weapon
        Destroy(heldWeapons[i]);
        heldWeapons.RemoveAt(i);
    }

    public void NewWeapon(GameObject weapon) {
        hasWeapon = true;
        weapon.transform.SetParent(transform);
        heldWeapons.Add(weapon);
        ChangeWeapon(heldWeapons.Count-1);
    }

    public void PickupWeapon(GameObject target) {

        //Add the weapon to the arsonal
        GameObject newWeapon = Instantiate(target.GetComponent<PickupItem>().GetItem(), transform.position, new Quaternion());
        newWeapon.transform.parent = gameObject.transform;
        newWeapon.GetComponent<Weapon>().GetControllerAndEquip();
        newWeapon.transform.localScale = new Vector3(2, 2, 0);
        NewWeapon(newWeapon);

        //Destroy the weapon on the ground
        target.GetComponent<PickupItem>().CleanUp();
    }

}
