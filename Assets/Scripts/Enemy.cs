using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    [SerializeField] protected bool dealDamageOnContact;
    [SerializeField] protected float visRange;
    [SerializeField] float damage;

    public TrackerController trackerController;
    // Start is called before the first frame update
    void Start()
    {
        damage = 20;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject closestPlayer = FindClosestPlayer();
        if (closestPlayer != null) {
            trackerController.SetTarget(closestPlayer.transform);
        }
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

    public float GetDamage() {
        return damage;
    }

}
