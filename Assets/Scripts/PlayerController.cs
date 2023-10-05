using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float dash;
    [SerializeField, Range(0,1)] private float damper;
    [SerializeField] private Weapon weapon;
    [SerializeField] private static double damage;
    private Vector2 direction;
    private Vector2 saved_direction;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        damage = 20d;
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

        rb.velocity *= Mathf.Pow(1f - damper, Time.deltaTime * 10f); ;
        if (Input.GetMouseButtonDown(0)) {
            weapon.Fire();
        }
        rb.velocity += direction * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J)) {
            Debug.Log(GetDamage());
        }
    }

    public static double GetDamage() {
        return damage;
    }

    // Method to increase the damage that the player deals using a weapon.
    public static void AddDamage(double BonusDamage) {
        damage += BonusDamage;
    }

}
