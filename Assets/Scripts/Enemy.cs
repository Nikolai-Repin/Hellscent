using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    [SerializeField] protected bool dealDamageOnContact;
    [SerializeField] protected float visRange;

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "player") {
            if (dealDamageOnContact) {
                other.GetComponent<Controller>().TakeDamage(1);
            }
        }
    }

    //Returns closest player in range
    public GameObject FindClosestPlayer(float range) {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, range);
        List<Collider2D> players = new List<Collider2D>();
        for (int i = 0; i < results.Length; i++) {
            if (results[i].transform.GetComponent<Controller>() != null) {
                players.Add(results[i]);
            }
        }
        if (results.Length > 0) {
            return FindClosest(players, transform.position);
        }
        return null;
    }

    //Returns closest player in visRange
    public GameObject FindClosestPlayer() {
        return FindClosestPlayer(visRange);
    }
}
