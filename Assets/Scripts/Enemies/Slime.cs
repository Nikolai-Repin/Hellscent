using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    private Phase curPhase;
    private Animator animator;

    // Start is called before the first frame update
    new void Start()
    {
        if (size <= 0) {
            Destroy(transform.gameObject);
        }
        dealDamageOnContact = true;
        intangible = false;
        trackerController.aiPath.maxSpeed = 5;
        animator = GetComponent<Animator>();
        randAttackDelay = Random.Range(0, 0.5F);
        lastAttackTime = Time.time;
        curPhase = Phase.Sleep;

        //Why wont the game call these???
        splitOffHealth = healthAmount/4;
        Debug.Log(healthAmount);
        Debug.Log(splitOffHealth);
        
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
                    lastAttackTime = Time.time + 0.5F;
                }
                break;
            }

            case Phase.ChargeReady: {
                trackerController.aiPath.maxSpeed = 0;
                if (Time.time >= lastAttackTime) {
                    lastAttackTime = Time.time + 0.1F;
                    trackerController.aiPath.maxSpeed = 500;
                    trackerController.aiPath.maxAcceleration = 500;
                    curPhase = Phase.ChargeDash;
                }
                break;
            }

            case Phase.ChargeDash: {
                if (Time.time >= lastAttackTime) {
                    lastAttackTime = Time.time + 0.3F;
                    trackerController.aiPath.maxAcceleration = 5;
                    curPhase = Phase.ChargeEnd;
                }
                break;
            }

            case Phase.ChargeEnd: {
                if (Time.time >= lastAttackTime) {
                    lastAttackTime = Time.time + 2;
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
                if (Time.time > lastAttackTime) {
                    GameObject splitOff;
                    Vector3 splitOffOffset = new Vector3(size, 0, 0);

                    splitOff = Instantiate(clone, transform.position, new Quaternion());
                    splitOff.GetComponent<Slime>().healthAmount = splitOffHealth;
                    splitOff.GetComponent<Slime>().size--;
                    splitOff.GetComponent<Slime>().trackerController.aiPath.maxSpeed = 5;
                    splitOff.GetComponent<Slime>().invulnTime = Time.time + 0.5F;
                    splitOff.transform.position -= splitOffOffset;

                    splitOff = Instantiate(clone, transform.position, new Quaternion());
                    splitOff.GetComponent<Slime>().healthAmount = splitOffHealth;
                    splitOff.GetComponent<Slime>().size--;
                    splitOff.GetComponent<Slime>().trackerController.aiPath.maxSpeed = 5;
                    splitOff.GetComponent<Slime>().invulnTime = Time.time + 0.5F;
                    splitOff.transform.position += splitOffOffset;
                    Destroy(transform.gameObject);
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
