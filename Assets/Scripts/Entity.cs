using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] protected bool invulnerable;
    [SerializeField] protected bool intangible;
    [SerializeField] public float healthAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Deals damage to entity if invulnerable, returns true if damage was dealt
    public virtual bool TakeDamage(float damage) {
        if (intangible) {
            return false;
        }

        if (!invulnerable) {
            healthAmount -= damage;
            if (healthAmount <= 0) {
                Die();
            }
        }
        return true;
   }

    //Finds the closest game object from a array of collider2D and their distance from Vector3 origin
    public GameObject FindClosest (Collider2D[] targets, Vector3 origin) {
        if (targets.Length > 0) {
            GameObject closest = targets[0].transform.gameObject;
            float closestLen = (targets[0].transform.position - origin).sqrMagnitude;
            float curLen = closestLen;

            for (int i = 1; i < targets.Length; i++) {
                curLen = (targets[i].transform.position - origin).sqrMagnitude;
                if (curLen < closestLen) {
                    closestLen = curLen;
                    closest = targets[i].transform.gameObject;
                }
            }

            return closest;
        }
        return null;
    }

    //Finds the closest game object from a list of collider2D and their distance from Vector3 origin
    public GameObject FindClosest (List<Collider2D> targets, Vector3 origin) {
        return FindClosest(targets.ToArray(), origin);
    }

    //Destroys the entity
    public virtual void Die () {
        Destroy(transform.gameObject);
    }

    public virtual float GetDamage() {
        return 0;
    }
    
}
