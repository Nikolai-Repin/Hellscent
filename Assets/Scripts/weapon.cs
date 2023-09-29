using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public int offset = 2;
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
        parent = transform.parent.gameObject;
        sr = GetComponent<SpriteRenderer>();
        controller = parent.GetComponent<Controller>();
        controller.heldWeapons.Add(GetComponent<Weapon>());
        controller.ChangeWeapon(controller.heldWeapons.Count-1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.transform.position;
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
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
            bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(bulletScript.projectileSpeed*Mathf.Cos(transform.rotation.eulerAngles.z*Mathf.Deg2Rad), bulletScript.projectileSpeed*Mathf.Sin(transform.rotation.eulerAngles.z*Mathf.Deg2Rad),0));
            bulletScript.creator = transform.gameObject;

            ammo--;
            cooldown = cooldownTime;
            return true;
        }

        return false;
    }


}