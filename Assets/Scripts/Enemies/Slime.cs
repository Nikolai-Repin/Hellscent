using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Slime : Enemy
{
    [SerializeField] public int size;
    [SerializeField] private GameObject clone;
    private float lastAttackTime;
    private float splitOffHealth;
    public float randAttackDelay;
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
    new void Start()
    {
        if (size <= 0) {
            Destroy(transform.gameObject);
        }
        Debug.Log("hewwo");
        dealDamageOnContact = true;
        intangible = false;
        trackerController.aiPath.maxSpeed = 5;
        animator = GetComponent<Animator>();
        randAttackDelay = Random.Range(0, 0.5F);
        lastAttackTime = Time.time;
        if (curPhase == null) {
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
                if (Time.time >= lastAttackTime && ((trackerController.target.transform.position - transform.position).sqrMagnitude) <= 500) {
                    curPhase = Phase.ChargeReady; 
                    lastAttackTime = Time.time + 0.2F;
                }
                break;
            }

            case Phase.ChargeReady: {
                trackerController.aiPath.maxSpeed = 0;
                trackerController.aiPath.maxAcceleration = 0;
                if (Time.time >= lastAttackTime) {
                    lastAttackTime = Time.time + 0.3F;
                    trackerController.aiPath.maxSpeed = 50;
                    trackerController.aiPath.maxAcceleration = 5;
                    

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
                if (Time.time >= lastAttackTime) {
                    lastAttackTime = Time.time + 2;
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
                if (Time.time > lastAttackTime && size > 1) {
                    GameObject splitOff;
                    Vector2 splitOffOffset = new Vector2(size, 0);

                    splitOff = Instantiate(clone, transform.position, new Quaternion());
                    var splitOffSlime = splitOff.GetComponent<Slime>();
                    splitOffSlime.healthAmount = splitOffHealth;
                    splitOffSlime.size--;
                    splitOffSlime.trackerController.aiPath.maxSpeed = 5;
                    splitOffSlime.invulnTime = Time.time + 0.5F;
                    splitOff.GetComponent<AIBase>().velocity2D += splitOffOffset*-1;
                    
                    splitOff = Instantiate(clone, transform.position, new Quaternion());
                    splitOffSlime = splitOff.GetComponent<Slime>();
                    splitOffSlime.healthAmount = splitOffHealth;
                    splitOffSlime.size--;
                    splitOffSlime.trackerController.aiPath.maxSpeed = 5;
                    splitOffSlime.invulnTime = Time.time + 0.5F;
                    splitOffSlime.curPhase = Phase.Splitting;
                    splitOff.GetComponent<AIBase>().velocity2D += splitOffOffset;

                    base.Die();
                }
                break;
            }

            case (Phase.Splitting): {
                if (Time.time > lastAttackTime) {
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
}
