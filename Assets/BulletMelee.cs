using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMelee : Bullet
{
    [SerializeField] private bool reflect;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Bullet" && other.GetComponent<Bullet>().getReflectable()) {
            other.gameObject.GetComponent<Bullet>().LaunchProjectile(transform.rotation);
        }
    }

    public void LaunchProjectile(Quaternion rotation) {
        transform.rotation = creator.transform.rotation;
        transform.position += creator.transform.position.normalized * creator.GetComponent<Weapon>().GetOffset();
    }
}
