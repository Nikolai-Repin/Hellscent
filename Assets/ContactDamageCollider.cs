using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamageCollider : MonoBehaviour
{
    private Enemy owner;

    // Start is called before the first frame update
    void Start() {
        owner = transform.parent.GetComponent<Enemy>();
    }

    protected void OnTriggerStay2D(Collider2D other) {
        owner.DealContactDamage(other);
    }
}
