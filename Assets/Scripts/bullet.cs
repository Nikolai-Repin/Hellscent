using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity {
    
    [SerializeField] private float maxLife; //How long a bullet should exist for, in seconds, I think.
    [SerializeField] private float bulletDamage; // How much damage a bullet should do
    [SerializeField] private float projectileSpeed; //How fast bullet move
    [SerializeField] private int pierce; //How many entities it should interact with
    [SerializeField] private bool reflectable; //Should it be flectable by melee weapons
    [SerializeField] private bool setDamage; 
    [SerializeField] private bool rotate;
    private LayerMask walls;

    public string team;
    protected GameObject creator;
    protected Weapon wc;
    private float lifeTime = 0f; //How long the bullet has existed for

    //Defines how the bullet should move when the bullet is first fired
    public virtual void LaunchProjectile(Quaternion rotation) {
        SetProjectileVelocity(rotation, projectileSpeed*wc.modProjectileSpeed);
    }

    //Defines how the bullet should move when the bullet is first fired using custom speed
    public virtual void LaunchProjectile(Quaternion rotation, float speed) {
        SetProjectileVelocity(rotation, speed);
    }

    //Sets bullet velocity based on rotation, using bullet speed
    public virtual void SetProjectileVelocity(Quaternion rotation) {
        SetProjectileVelocity(rotation, projectileSpeed);
    }

    //Sets bullet velocity based on rotation and how fast it should move
    public virtual void SetProjectileVelocity(Quaternion rotation, float strength) {
        Vector3 newVelocity = new Vector3(0,0,0);
        newVelocity.x = strength * Mathf.Cos(rotation.eulerAngles.z * Mathf.Deg2Rad);
        newVelocity.y = strength * Mathf.Sin(rotation.eulerAngles.z * Mathf.Deg2Rad);
        if (rotate) {transform.rotation = rotation;}
        GetComponent<Rigidbody2D>().velocity = newVelocity;
    }

    void Start() {
    }

    void Update() {
        lifeTime += Time.deltaTime; //Update bullet lifetime

        //This being called every frame could be laggy, it's likely that there's a better way to do this
        //Kill bullet if it's too old
        if (lifeTime >= maxLife) {
            Die();
        }
    }

    protected void OnTriggerStay2D(Collider2D other) 
    {
        
        //I need to set up teams or something of the like for this, I want bullets to be able to belong to enemies
        if (other.gameObject.GetComponent<Entity>() != null && other.gameObject.GetComponent<Bullet>() == null &&  other.gameObject.tag != team) {
            if (other.gameObject.GetComponent<Entity>().TakeDamage(bulletDamage)) {
                pierce--;
            }
        }

        if (other.gameObject.layer == LayerMask.GetMask("Walls")) { //Hardcoding because I don't have the time today to set up a way to handle what bullets should interact with, maybe check if they have the same parent?
            pierce = 0;

        }

        if (pierce <= 0) {
            Die();
        }
    }
    
    public void UpdateCreator(GameObject c) {
        creator = c;
        wc = creator.transform.GetComponent<Weapon>();
    }

    // Sets values like damage and bullet size whenever a bullet spawns
    public void SetStartingValues() {
        if (setDamage) {
            bulletDamage = creator.GetComponent<Weapon>().GetDamage();
        } else {
            bulletDamage += creator.GetComponent<Weapon>().GetDamage();
        }
        //changes the scale based on damage (change the values of the denominators if you wanna change how much the size scales).
        transform.localScale += new Vector3(bulletDamage/10, bulletDamage/10, 0f);
    }

    public void SetStartingValues(float scale, float maxLife, float damage, float projectileSpeed, int pierce, bool reflectable, bool setDamage, bool rotate) {
        this.transform.localScale = Vector3.one*scale;
        this.maxLife = maxLife;
        bulletDamage = (setDamage) ? bulletDamage : bulletDamage + damage;
        this.projectileSpeed = projectileSpeed;
        this.pierce = pierce;
        this.reflectable = reflectable;
        this.setDamage = setDamage;
        this.rotate = rotate;
        this.lifeTime = 0f;
    }

    public void AddDamage(float damage, bool updateScale) {
        bulletDamage += creator.GetComponent<Weapon>().GetDamage();
        if (updateScale) {transform.localScale += new Vector3(damage/30, damage/30, 0f);}
    }

    public float getProjectileSpeed() {
        return projectileSpeed;
    }

    public bool getReflectable() {
        return reflectable;
    }

    public void Reflected(string newTeam) {
        reflectable = false;
        team = newTeam;
        GetComponent<Rigidbody2D>().velocity *= -1;
        lifeTime = 0;
    }

    public override void Die() {
        Destroy(gameObject);
    }
}