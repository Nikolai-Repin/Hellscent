using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    public float maxLife; //How long a bullet should exist for, in seconds, I think.
    public float damage;
    public int projectileSpeed;

    public GameObject creator;
    private float lifeTime = 0f;


    void Update() {
        lifeTime += Time.deltaTime;

        //This being called every frame could be laggy, it's likely that there's a better way to do this
        if (lifeTime >= maxLife) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            Health health =  other.gameObject.GetComponent<Health>();
            health.TakeDamage(20);
        }
        if (other.GetComponent<Bullet>() == null) { //Hardcoding because I don't have the time today to set up a way to handle what bullets should interact with, maybe check if they have the same parent?
            Destroy(gameObject);
        }
    }
}