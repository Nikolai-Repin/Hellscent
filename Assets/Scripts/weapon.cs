using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int weight;

    [SerializeField] public float offset = 2F;
    [SerializeField] public GameObject projectileType;
    [SerializeField] public float kickback = 0F;

    [SerializeField] public float cooldownTime = 0.5F;
    [SerializeField] public int bullets = 1;
    [SerializeField] public float accuracy = 10.0F;
    [SerializeField] public int clip = 1;
    [SerializeField] public int clipDelay;

    [SerializeField] private float weaponDamage;

    [SerializeField] public bool useMana;
    [SerializeField] public float manaCost = 1.0F;
    [SerializeField] private float maxMana;
    [SerializeField] private float mana;
    [SerializeField] private float manaRechargeDelay = 1;

    [Space]
    [SerializeField] public bool canCharge;
    [SerializeField] public float chargeTime;
    [SerializeField] public float incrementTime;
    [SerializeField] public float extraManaUse;
    [SerializeField] private float extraDamage;
    [SerializeField] private float extraKickback;
    [SerializeField] private int extraClip;

    [Space]
    [SerializeField] private float lastFireTime;

    
    [SerializeField] protected float cooldown;
    protected GameObject parent;
    protected SpriteRenderer sr;
    protected Entity controller; //change to Player controller if needed
    protected Vector2 target;

    //Rand Stats
    public float quality = 0.0F;
    [SerializeField] public bool randomize;
    public float modCooldownTime = 1.0F;
    public float modDamage = 0F;
    public float modProjectileSpeed = 1.0F;
    public float modAccuracy = 1.0F;
    public int modBullets = 0;
    public float modManaCost = 1.0F;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        cooldownTime *= modCooldownTime;
        cooldown = 0;

        if (transform.parent != null) {
            parent = transform.parent.gameObject;
            
            if (parent.GetComponent<PlayerController>() != null) {
                parent.GetComponent<PlayerController>().NewWeapon(transform.gameObject);
                SetTarget(Input.mousePosition);
                lastFireTime = Time.time;
                mana = maxMana;
                useMana = true;
            } else {
                useMana = false;
            }
            controller = parent.GetComponent<Entity>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (parent.GetComponent<PlayerController>() != null && parent.GetComponent<PlayerController>().alive) {
            UpdateTarget();
        }

        if (transform.parent != null) {
            UpdateAngleAndPosition(target);
        }

        //If weapon is marked for randomization, randomize modifiers. Dangerous to call more than once on a weapon
        if (randomize) {
            modManaCost = RandomizeMods(1.0F, quality);
            randomize = false;
        }

        if (useMana && Time.time>lastFireTime) {
            if (mana < maxMana) {
                mana += parent.GetComponent<PlayerController>().GetManaRechargeSpeed()*Time.deltaTime;
            }

            if (mana > maxMana) {
                mana = maxMana;
            }
        } 

        cooldown -= Time.deltaTime;
    }

    //Fires the selected projectile
    public bool Fire(bool useCooldown = true)
    {
        if (useCooldown && (cooldown > 0 || (useMana && mana < manaCost))) {return false;}

        if (useCooldown && clip > 1) {
            StartCoroutine(FireClip(clip)); 
            lastFireTime = Time.time + manaRechargeDelay;
            if (useMana) {mana -= manaCost;}
            cooldown = cooldownTime;
            return true;
        }

        for (int i = 0; i < bullets+modBullets; i++)
        {
            GameObject bullet = Instantiate(projectileType, transform.position, new Quaternion());
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.UpdateCreator(transform.gameObject);
            bulletScript.team = parent.tag;

            Vector3 inaccuracy = new Vector3(0, 0, Random.Range(-1.0F* accuracy*modAccuracy, accuracy*modAccuracy));
            Quaternion fireAngle = Quaternion.Euler(transform.rotation.eulerAngles + inaccuracy);

            bulletScript.LaunchProjectile(fireAngle);
            bullet.GetComponent<Rigidbody2D>().velocity += parent.GetComponent<Rigidbody2D>().velocity.normalized;

            bulletScript.SetStartingValues();
        }

        lastFireTime = Time.time + manaRechargeDelay;
        if (useCooldown && useMana) {mana -= manaCost;}
        cooldown = cooldownTime;
        if (!useCooldown) {
            transform.parent.gameObject.GetComponent<PlayerController>().doKickback();
        }

        return true;
    }

    public bool Fire(float timeCharged) {
        if (timeCharged > chargeTime) {
            timeCharged = chargeTime;
        }
        int maxIncrements = (int) (chargeTime / incrementTime);
        int increments = (int) (timeCharged / incrementTime);
        float incrementRatio = timeCharged / chargeTime;
        StartCoroutine(resetVars(manaCost, weaponDamage, kickback, clip, (clip + (int) (extraClip * incrementRatio)) * clipDelay + 1));
        float manaCostDiff = extraManaUse * incrementRatio;
        if (manaCost + manaCostDiff > mana) {
            if (manaCost > mana) {
                return false;
            }
            return Fire((mana - manaCost) * chargeTime / extraManaUse);
        }
        manaCost += manaCostDiff;
        weaponDamage += extraDamage * incrementRatio;
        kickback += extraKickback * incrementRatio;
        clip += (int) (extraClip * incrementRatio);
        return Fire();
    }

    IEnumerator FireClip(int clipSize) {
        if (clipSize > 0) {
            for (int i = 0; i < clipSize; i++) {
                for (int j = 0; j < clipDelay; j++) {
                    yield return null;
                }
                Fire(false);
            }
        }
    }

    IEnumerator resetVars(float m, float d, float k, int c, int frames = 1) {
        for (int i = 0; i < frames; i++) {
            yield return null;
        }
        manaCost = m;
        weaponDamage = d;
        kickback = k;
        clip = c;
    }

    public bool GetControllerAndEquip() {
        if (transform.parent.gameObject.GetComponent<PlayerController>() != null) {
            controller = parent.GetComponent<Entity>();
            return true;
        }
        return false;
    }

    public void UpdateAngleAndPosition(Vector3 targetPosition) {

        //Setting position and angle
        transform.position = parent.transform.position;
       var dir = targetPosition - transform.parent.position;
        var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle + 90, Vector3.forward);
        
        transform.position += dir.normalized * offset;

        //Flip gun so it won't be upside down when aiming left
        if (parent.tag != "player") {
            if (angle < 0) {
                sr.flipY = true;
            } else {
                sr.flipY = false;
            }
        }
        //Render the weapon on top of the wielder
        sr.sortingOrder = parent.GetComponent<SpriteRenderer>().sortingOrder + 20;
        
    }

    public void OnTransformParentChanged() {
        parent = transform.parent.gameObject;
    }

    public float RandomizeMods(float variance, float quality) {
        modCooldownTime += (Random.Range(-variance, variance) + quality) *1.0F; 
        modDamage += (Random.Range(-variance, variance) + quality) *1.0F; 
        modProjectileSpeed += (Random.Range(-variance, variance) + quality) *1.0F; 
        modAccuracy += (Random.Range(-variance, variance) + quality) *1.0F; 

        while (Random.Range(0, variance*10) <= quality && modBullets < 3) {
            modBullets++;
        }

        //Replace the number at the end with how many modifiers there are - Currently 5
        return 1.0F+(((modCooldownTime-1)+(modDamage-1)+(modProjectileSpeed-1)+((modAccuracy*-1.0F)-1)+modBullets)/5.0F);
    }

    public void UpdateTarget() {
        if (transform.parent != null) {
            parent = transform.parent.gameObject;
            if (transform.parent.gameObject.GetComponent<PlayerController>() != null) {
                SetTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }

    //Setters
    public void SetTarget(Vector2 target) {this.target = target;}
    public void SetMaxMana(float a) {maxMana = a;}

    //Getters
    public float GetOffset() {
        return offset;
    }

    public float GetCoolDown() {
        return cooldown;
    }

    public float GetManaCost() {
        return manaCost*modManaCost;
    }

    public float GetDamage() {
        return weaponDamage;
    }

    public bool CanShoot() {
        return cooldown <= 0;
    }

    public void AddDamage(float bonusDamage) {
        weaponDamage += bonusDamage;
    }

    public GameObject GetParent() {
        return parent;
    }

    //Returns percentage of current mana out of maxMana
    public float GetManaPercent() {
        return mana/maxMana;
    }

    public float GetMaxMana() {
        return maxMana;
    }

    public float GetMana() {
        return mana;
    }

    public void AddMaxMana(float a) {
        maxMana += a;
    }

        public int GetWeight() {
        return weight;
    }

    public void StopRecharge() {
        lastFireTime = Time.time + manaRechargeDelay;
    }

}