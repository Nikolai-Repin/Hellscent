using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private float lastAttackTime;
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
    void Start()
    {
        curPhase = Phase.Sleep;
    }

    // Update is called once per frame
    void Update()
    {
        switch (curPhase) {
            case Phase.Wander: {
                if (((trackerController.target.transform.position - transform.position).sqrMagnitude) <= 100) {
                    curPhase = Phase.ChargeReady; 
                    lastAttackTime = Time.time + 0.5;
                }
                break;
            }

            case Phase.ChargeReady: {
                trackerController.aiPath.maxSpeed = 0;
                if (Time.time >= lastAttackTime) {
                    curPhase = phase.ChargeDash;
                }
            }

            case Phase.ChargeDash: {
                trackerController.aiPath.maxSpeed = 20;

            }

            case Phase.Sleep: {
                GameObject closestPlayer = FindClosestPlayer();
                if (closestPlayer != null) {
                    trackerController.SetTarget(closestPlayer.transform);
                    lastAttackTime = Time.time + 2;
                    curPhase = Phase.Wander;
                }
                break;
            }
        }
    }
}
