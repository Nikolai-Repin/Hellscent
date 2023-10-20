using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] protected bool vulnerable;
    [SerializeField] protected float healthAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Deals damage to entity if vulnerable, returns true if damage was dealt
    public bool TakeDamage(float damage) {
        if (vulnerable) {
            healthAmount -= damage;
            if (healthAmount <= 0) {
                Die();
            }
            Debug.Log("Damaged");
            return true;
        }
        return (false);
   }

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
        } else {
            Debug.Log("targets is empty");
        }
        return null;
    }

    public GameObject FindClosest (List<Collider2D> targets, Vector3 origin) {
        return FindClosest(targets.ToArray(), origin);
    }

    public virtual void Die () {
        Destroy(transform.gameObject);
    }
}
