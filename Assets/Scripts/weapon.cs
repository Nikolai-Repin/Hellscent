using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public float offset = 2F;
    [SerializeField] public int ammo = -1;
    [SerializeField] public GameObject projectileType;
    [SerializeField] public float cooldownTime = 0.5F;
    [SerializeField] public float kickback = 0F;

    private float cooldown;
    private GameObject parent;
    private SpriteRenderer sr;
    private Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (transform.parent != null) {
            parent = transform.parent.gameObject;
            if (transform.parent.gameObject.GetComponent<Controller>() != null) {GetControllerAndEquip();}
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.parent != null && transform.parent.gameObject.GetComponent<Controller>() != null) {UpdateAngleAndPosition(Input.mousePosition);}

        cooldown -= Time.deltaTime;
    }

    //Fires the selected projectile
    public bool Fire()
    {
        if (ammo == 0) {return false;}
        
        if (cooldown <= 0) {
            GameObject bullet = Instantiate(projectileType, transform.position, new Quaternion());
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.creator = transform.gameObject;
            bulletScript.LaunchProjectile(transform.rotation);
            
            ammo--;
            cooldown = cooldownTime;
            return true;
        }

        return false;
    }

    public float GetOffset() {return offset;}

    public bool GetControllerAndEquip() {
        if (transform.parent.gameObject.GetComponent<Controller>() != null) {
            controller = parent.GetComponent<Controller>();
            controller.NewWeapon(transform.gameObject);
            return true;
        }
        return false;
    }

    public void UpdateAngleAndPosition(Vector3 targetPosition) {

        //Setting position and angle
        transform.position = parent.transform.position;
        var dir = targetPosition - Camera.main.WorldToScreenPoint(transform.parent.position);
        var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle + 90, Vector3.forward);
        transform.position += dir.normalized * offset;

        //Flip gun so it won't be upside down when aiming left
        if (angle < 0) {
            sr.flipY = true;
        } else {
            sr.flipY = false;
        }
    }

    public void OnTransformParentChanged() {
        parent = transform.parent.gameObject;
    }
}