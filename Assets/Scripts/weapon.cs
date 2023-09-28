using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int offset = 16;
    public int ammo;
    //public int projectileSpeed;
    public GameObject projectileType;
    
    //public 

    private GameObject parent;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        sr = GetComponent<SpriteRenderer>();
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
    }

    public bool Fire()
    {
        if (ammo == 0) {return false;}
        
        
        GameObject bullet = Instantiate(projectileType, transform.position, new Quaternion());
        bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(bullet.GetComponent<Bullet>().projectileSpeed*Mathf.Cos(transform.rotation.eulerAngles.z*Mathf.Deg2Rad), bullet.GetComponent<Bullet>().projectileSpeed*Mathf.Sin(transform.rotation.eulerAngles.z*Mathf.Deg2Rad),0));
    
        ammo--;
        return true;
    }


}