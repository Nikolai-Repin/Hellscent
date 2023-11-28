using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ContactDamageCollider : MonoBehaviour
{
    private Enemy owner;
    private enum DetectionType {
        damage = 1,
        trigger = 2,
    }
    [SerializeField] private DetectionType detectionType;
    [SerializeField] private bool PushOtherEntities;

    // Start is called before the first frame update
    void Start() {
        owner = transform.parent.GetComponent<Enemy>();
        Physics2D.IgnoreCollision(transform.parent.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    protected void OnTriggerStay2D(Collider2D other) {
        if (PushOtherEntities && other.GetComponent<ContactDamageCollider>() != null) {
            //Give force in the negative direction
            float forceMulti = 0.5f;

            Vector2 pushVector = (-1 * (other.transform.position - transform.position).normalized * forceMulti);
            owner.GetComponent<AIBase>().velocity2D += pushVector;
        }
        switch (detectionType) {
            case (DetectionType.damage): {
                owner.DealContactDamage(other);
                break;
            } 

            case (DetectionType.trigger): {
                owner.TriggerEvent(other);
                break;
            }

            default: {
                owner.DealContactDamage(other);
                break;
            }
        }
        
    }
}
