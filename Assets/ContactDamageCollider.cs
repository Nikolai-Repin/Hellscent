using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamageCollider : MonoBehaviour
{
    private Enemy owner;
    private enum DetectionType {
        damage = 1,
        playerTrigger = 2,
    }
    [SerializeField] private DetectionType detectionType;

    // Start is called before the first frame update
    void Start() {
        owner = transform.parent.GetComponent<Enemy>();
    }

    protected void OnTriggerStay2D(Collider2D other) {
        switch (detectionType) {
            case (DetectionType.damage): {
                owner.DealContactDamage(other);
                break;
            } 

            case (DetectionType.playerTrigger): {
                owner.TriggerEvent(other);
                break;
            }
        }
        
    }
}
