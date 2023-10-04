using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMelee : Bullet
{
    [SerializeField] private bool reflect;

    void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
        if (other.gameObject.tag == "Bullet" && other.GetComponent<Bullet>().getReflectable()) {
            other.gameObject.GetComponent<Bullet>().SetProjectileVelocity(transform.rotation, other.gameObject.GetComponent<Bullet>().getProjectileSpeed());
        }
    }

    public override void LaunchProjectile(Quaternion rotation) {
        transform.rotation = rotation;
        float v = (creator.GetComponent<Weapon>().GetOffset() * 2);
        transform.position += transform.right * v;
    }
}