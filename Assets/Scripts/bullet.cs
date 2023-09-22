using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public int maxLife; //How long a bullet should exist for

    public int projectileSpeed;

    private float lifeTime = 0f;

    void Update() {
        lifeTime += Time.deltaTime;

        //This being called every frame could be laggy, it's likely that there's a better way to do this
        if (lifeTime >= maxLife) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        Destroy(gameObject);
    }

    
}
