using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    [SerializeField] private double damage;
    public int projectileSpeed;
    public int maxLife; //How long a bullet should exist for

    private float lifeTime = 0f;

    void Start() {
        damage = Controller.GetDamage();
    }

    void Update() {
        lifeTime += Time.deltaTime;
        //This being called every frame could be laggy, it's likely that there's a better way to do this
        if (lifeTime >= maxLife) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Enemy") {
            // Enemy Health doesn't work for some reason, so I commented it for now.

            //Health health =  other.gameObject.GetComponent<Health>();
            //health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

}