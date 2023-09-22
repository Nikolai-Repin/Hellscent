using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public int maxLife;
    private int lifeTime = 0;

    void Update() {
        //lifeTime++;
        if (lifeTime >= maxLife) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        Destroy(gameObject);
    }
}
