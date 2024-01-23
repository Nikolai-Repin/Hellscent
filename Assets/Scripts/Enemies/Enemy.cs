using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    [SerializeField] protected float visRange;
    [SerializeField] public float iFrames;
    [SerializeField] private int weight;
    public float invulnTime;

    public TrackerController trackerController;

    // Update is called once per frame
    protected virtual void Update()
    { 
        base.Update();
        GameObject closestPlayer = FindClosestPlayer();
        if (closestPlayer != null && trackerController != null) {
            trackerController.SetTarget(closestPlayer.transform);
        }
    }

    //Returns closest player in range
    public GameObject FindClosestPlayer(float range) {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, range);
        List<Collider2D> players = new List<Collider2D>();
        for (int i = 0; i < results.Length; i++) {
            if (results[i].transform.GetComponent<PlayerController>() != null && results[i].transform.GetComponent<PlayerController>().alive) {
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

    public virtual void TriggerEvent(Collider2D other) {
        return;
    }

    public override bool TakeDamage(float damage) {
        if (Time.time < invulnTime) {
            return false;
        }
        return base.TakeDamage(damage);
    }
    public int GetWeight() {
        return weight;
    }
}
