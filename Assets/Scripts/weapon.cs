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

    protected float cooldown;
    private GameObject parent;
    private SpriteRenderer sr;
    private Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        sr = GetComponent<SpriteRenderer>();
        controller = parent.GetComponent<Controller>();
        controller.NewWeapon(GetComponent<Weapon>());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.transform.position;
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.parent.position);
        var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle + 90, Vector3.forward);
        transform.position += dir.normalized * offset;
        if (angle < 0)
        {
            sr.flipY = true;
        } else {
            sr.flipY = false;
        }

        cooldown -= Time.deltaTime;
    }

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

    public float getCoolDown(){return cooldown;}

}