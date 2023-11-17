using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Slime : Enemy
{
    [SerializeField] public int size;
    [SerializeField] private GameObject clone;
    [SerializeField] private bool spawnChildren;
    private float lastAttackTime;
    private float splitOffHealth;
    private float randAttackDelay;
    private bool isChildSlime;
    public enum Phase
    {
        Death = -1,
        Sleep = 0,
        Wander = 1,
        ChargeReady = 2,
        ChargeDash = 3,
        ChargeEnd = 4,
        Splitting = 5,
    }
    public Phase curPhase;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Register();
        if (size <= 0) {
            Destroy(transform.gameObject);
        }
        dealDamageOnContact = true;
        trackerController.aiPath.maxSpeed = 5;
        animator = GetComponent<Animator>();
        randAttackDelay = Random.Range(0, 0.5F);
        lastAttackTime = Time.time;
        if (!isChildSlime) {
            curPhase = Phase.Sleep;
        }
        
        splitOffHealth = healthAmount/4;
        
        float scale = 0+((size)*0.5F);
        transform.localScale = new Vector3(scale, scale, 1);
    }

    // Update is called once per frame
    new void Update()
    {
        switch (curPhase) {
            case Phase.Wander: {
                //Check if enough time has passed to attack and if close enough
                if (Time.time >= lastAttackTime && ((trackerController.target.transform.position - transform.position).sqrMagnitude) <= 500) {
                    //Switch to charge ready and prep for attack
                    curPhase = Phase.ChargeReady; 
                    trackerController.aiPath.maxSpeed = 0;
                    trackerController.aiPath.maxAcceleration = 0;
                    lastAttackTime = Time.time + 0.2F;
                }
                break;
            }

            case Phase.ChargeReady: {
                //For some ungodly reason, slimes are invulnerable while charging. For now, it's a feature.
                if (Time.time >= lastAttackTime) {

                    //A* caps velocity to maxSpeed, this gets around that
                    lastAttackTime = Time.time + 0.3F;
                    trackerController.aiPath.maxSpeed = 50;
                    trackerController.aiPath.maxAcceleration = 5;
                    
                    //Applying force to slime for charge
                    float forceMulti = 50f;
                    Vector2 pushVector = ((trackerController.target.transform.position - transform.position).normalized * forceMulti);
                    Debug.Log(pushVector/forceMulti);
                    Debug.Log(trackerController.target.transform.position - transform.position);
                    GetComponent<AIBase>().velocity2D += pushVector;

                    curPhase = Phase.ChargeEnd;
                }
                break;
            }

            case Phase.ChargeEnd: {
                //Compares time so slime won't be stopped mid charge
                if (Time.time >= lastAttackTime) {
                    lastAttackTime = Time.time + 2;

                    //Resets the slime back to wander
                    trackerController.aiPath.maxAcceleration = 5;
                    trackerController.aiPath.maxSpeed = 5;
                    curPhase = Phase.Wander;
                }
                break;
            }

            case Phase.Sleep: {
                GameObject closestPlayer = FindClosestPlayer();
                if (closestPlayer != null) {
                    trackerController.SetTarget(closestPlayer.transform);
                    lastAttackTime = Time.time + 2 + randAttackDelay;
                    curPhase = Phase.Wander;
                }
                break;
            }

            case (Phase.Death): {
                if (spawnChildren && Time.time > lastAttackTime && size > 1) {
                    Vector2 splitOffOffset = new Vector2(size, 0);
                    SpawnChild(splitOffOffset*-1);
                    SpawnChild(splitOffOffset);
                }
                base.Die();
                break;
            }

            case (Phase.Splitting): {
                intangible = true;
                if (Time.time > invulnTime) {
                    intangible = false;
                    GetComponent<AIPath>().enabled = true;
                    curPhase = Phase.Wander;
                }
                break;
            }
        }
    }

    public override void Die() {
        dealDamageOnContact = false;
        intangible = true;
        trackerController.aiPath.maxSpeed = 0;
        curPhase = Phase.Death;
        lastAttackTime = Time.time + 0.01F;
        animator.SetInteger("Phase", -1);
    }

    public override void DealContactDamage(Collider2D other) {
        if (other.gameObject.tag == "player") {
            if (dealDamageOnContact) {
                other.GetComponent<PlayerController>().TakeDamage(1);
            }
        }
    }

    public void SetChildStats(float health) {
        isChildSlime = true;
        healthAmount = health;
        size--;
        trackerController.aiPath.maxSpeed = 5;
        invulnTime = Time.time + 0.5F;
        curPhase = Phase.Splitting;
        intangible = false;
    }

    private void SpawnChild(Vector2 splitVelocity) {
        GameObject splitOff = Instantiate(clone, transform.position, new Quaternion());
        Slime splitOffSlime = splitOff.GetComponent<Slime>();
        SetChildStats(splitOffHealth);
        splitOff.GetComponent<AIBase>().velocity2D += splitVelocity;
    }
}
