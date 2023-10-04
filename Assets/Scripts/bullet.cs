using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    [SerializeField] private float maxLife; //How long a bullet should exist for, in seconds, I think.
    [SerializeField] private float damage;
    [SerializeField] private float projectileSpeed; //How fast bullet move
    [SerializeField] private int pierce; //How many entities it should inte
    [SerializeField] private bool reflectable; //Should it be flectable by melee weapons

    public GameObject creator; //Who created this bullet
    private float lifeTime = 0f; //How long the bullet has existed for

    public virtual void LaunchProjectile(Quaternion rotation) {
        GetComponent<Rigidbody2D>().velocity = (new Vector3(projectileSpeed*Mathf.Cos(rotation.eulerAngles.z*Mathf.Deg2Rad), projectileSpeed*Mathf.Sin(rotation.eulerAngles.z*Mathf.Deg2Rad),0)); //This line is ***rough*** but I can't be bothered to add more variables to this
    }

    public virtual void SetProjectileVelocity(Quaternion rotation) {
        GetComponent<Rigidbody2D>().velocity = new Vector3(projectileSpeed * Mathf.Cos(rotation.eulerAngles.z * Mathf.Deg2Rad), projectileSpeed * Mathf.Sin(rotation.eulerAngles.z * Mathf.Deg2Rad), 0);
    }
    public virtual void SetProjectileVelocity(Quaternion rotation, float strength) {
        GetComponent<Rigidbody2D>().velocity = new Vector3(strength * Mathf.Cos(rotation.eulerAngles.z * Mathf.Deg2Rad), strength * Mathf.Sin(rotation.eulerAngles.z * Mathf.Deg2Rad), 0);
    }

    void Update() {
        lifeTime += Time.deltaTime; //Update bullet lifetime

        //This being called every frame could be laggy, it's likely that there's a better way to do this
        //Kill bullet if it's too old
        if (lifeTime >= maxLife) {
            Destroy(gameObject);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        //I need to set up teams or something of the like for this, I want bullets to be able to belong to enemies
        if (other.gameObject.tag == "Enemy") {
            Health health =  other.gameObject.GetComponent<Health>();
            health.TakeDamage(damage);
            pierce--;
        }
        if (other.gameObject.tag == "Wall") { //Hardcoding because I don't have the time today to set up a way to handle what bullets should interact with, maybe check if they have the same parent?
            pierce--;
        }
        if (pierce <= 0) {
            Destroy(gameObject);
        }
    }

    public float getProjectileSpeed() {return projectileSpeed;}
    public bool getReflectable() {return reflectable;}
}