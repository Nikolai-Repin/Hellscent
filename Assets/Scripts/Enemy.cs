using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    [SerializeField] protected bool dealDamageOnContact;
    [SerializeField] protected float visRange;
    
    [SerializeField] public float iFrames;
    public float invulnTime;

    public TrackerController trackerController;
    // Start is called before the first frame update
    void Start()
    {
        //damage = 20;
    }

    // Update is called once per frame
    protected void Update()
    {
        GameObject closestPlayer = FindClosestPlayer();
        if (closestPlayer != null) {
            trackerController.SetTarget(closestPlayer.transform);
        }
    }
    /*
    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "player") {
            if (dealDamageOnContact) {
                other.GetComponent<PlayerController>().TakeDamage(1);
            }
        }
    }
    */

    //Returns closest player in range
    public GameObject FindClosestPlayer(float range) {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, range);
        List<Collider2D> players = new List<Collider2D>();
        for (int i = 0; i < results.Length; i++) {
            if (results[i].transform.GetComponent<PlayerController>() != null) {
                players.Add(results[i]);
            }
        }
        if (results.Length > 0) {
            FindClosest(players, transform.position);
            return FindClosest(players, transform.position);
        }
        return null;
    }

    //Returns closest player in visRange
    public GameObject FindClosestPlayer() {
        return FindClosestPlayer(visRange);
    }

    public virtual void DealContactDamage(Collider2D other) {
        if (other.gameObject.tag == "player") {
            if (dealDamageOnContact) {
                other.GetComponent<PlayerController>().TakeDamage(1);
            }
        }
    }

    public override bool TakeDamage(float damage) {
        if (intangible || Time.time < invulnTime) {
            return false;
        }

        if (!invulnerable) {
            healthAmount -= damage;
            if (healthAmount <= 0) {
                invulnTime = Time.time + iFrames;
                Die();
            }
        }
        return true;
   }

}