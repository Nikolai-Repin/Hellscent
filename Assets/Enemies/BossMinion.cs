using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMinion : Enemy
{
    private float lastAttackTime;
    private enum Phase {
        Death = -1,
        Live = 1,
    }
    private Phase curPhase;

    // Start is called before the first frame update
    void Start()
    {
        lastAttackTime = Time.time;
        curPhase = Phase.Live;
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        switch (curPhase) {
            case (Phase.Live): {
                GameObject closestPlayer = FindClosestPlayer();
                if (closestPlayer != null) {
                    trackerController.SetTarget(closestPlayer.transform);
                }
                break;
            }

            case (Phase.Death): {
                if (Time.time > lastAttackTime) {
                    Destroy(transform.gameObject);
                }
                break;
            }
        }
    }

    public override void Die() {
        dealDamageOnContact = false;
        intangible = true;
        trackerController.aiPath.maxAcceleration = 0;
        curPhase = Phase.Death;
        lastAttackTime = Time.time + 0.01F;
        //animator.SetInteger("Phase", -1);
    }

    public override void DealContactDamage(Collider2D other) {
        if (other.gameObject.tag == "player") {
            if (dealDamageOnContact) {
                if (other.GetComponent<PlayerController>().TakeDamage(1)) {
                    Die();
                }
            }
        }
        if (other.gameObject.layer == 3) {
            trackerController.aiPath.maxSpeed = 0;
            Die();
        }
    }
}
