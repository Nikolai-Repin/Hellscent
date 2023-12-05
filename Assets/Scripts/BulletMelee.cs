using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMelee : Bullet
{

    protected new void OnTriggerStay2D(Collider2D other) {
        base.OnTriggerStay2D(other);
        if (other.gameObject.tag == "Bullet" && other.GetComponent<Bullet>().getReflectable() && team != other.gameObject.GetComponent<Bullet>().team) {
            other.gameObject.GetComponent<Bullet>().Reflected(team);
        }
    }

    public override void LaunchProjectile(Quaternion rotation) {
        transform.rotation = rotation;
        float v = (wc.GetOffset());
        transform.position += transform.right * v;
        GetComponent<Rigidbody2D>().velocity = creator.transform.parent.GetComponent<Rigidbody2D>().velocity;
    }
}