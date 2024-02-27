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
    [SerializeField] public float clipDelay;

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

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

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
            // Checks weather a clip shot should be fired
            StartCoroutine(FireClip(clip)); 
            lastFireTime = Time.time + manaRechargeDelay;
            if (useMana) {mana -= manaCost;}
            cooldown = cooldownTime;
            return true;
        }

        for (int i = 0; i < bullets; i++)
        {
            GameObject bullet = Instantiate(projectileType, transform.position, new Quaternion());
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.UpdateCreator(transform.gameObject);
            bulletScript.team = parent.tag;

            Vector3 inaccuracy = new Vector3(0, 0, Random.Range(-1.0F * accuracy, accuracy));
            Quaternion fireAngle = Quaternion.Euler(transform.rotation.eulerAngles + inaccuracy);

            bulletScript.LaunchProjectile(fireAngle);
            //bulletScript.damage = this.weaponDamage;
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
        //Overloaded method for fire that takes care of charged weapons
        if (timeCharged > chargeTime) {
            timeCharged = chargeTime;
        }
        // Determines the amount of upgrades based on charge time
        int maxIncrements = (int) (chargeTime / incrementTime);
        int increments = (int) (timeCharged / incrementTime);
        float incrementRatio = timeCharged / chargeTime;
        float manaCostDiff = extraManaUse * incrementRatio;
        if (manaCost + manaCostDiff > mana) {
            // Checks is mana cost is too high and if so, tries again with tweaked charge time
            if (manaCost > mana) {
                return false;
            }
            return Fire((mana - manaCost) * chargeTime / extraManaUse);
        }
        Debug.Log((clip + (int) (extraClip * incrementRatio)) * clipDelay);
        // Schedules reset of weapon values after charge shot has been executed
        StartCoroutine(resetVars(manaCost, weaponDamage, kickback, clip, (clip + (int) (extraClip * incrementRatio)) * clipDelay));
        // Modifies values before firing
        manaCost += manaCostDiff;
        weaponDamage += extraDamage * incrementRatio;
        kickback += extraKickback * incrementRatio;
        clip += (int) (extraClip * incrementRatio);
        return Fire();
    }

    IEnumerator FireClip(int clipSize) {
        // Fires bullets in sequence with delay in between
        if (clipSize > 0) {
            for (int i = 0; i < clipSize; i++) {
                yield return new WaitForSeconds(clipDelay);
                Fire(false);
            }
        }
    }

    IEnumerator resetVars(float m, float d, float k, int c, float delay = 0) {
        // Resets weapon stats after set time
        if (delay > 0) {
            yield return new WaitForSeconds(delay);
        }
        yield return null;
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
        return manaCost;
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
    
    public float Damage {
        get { return weaponDamage; }
        set { weaponDamage = value; }
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