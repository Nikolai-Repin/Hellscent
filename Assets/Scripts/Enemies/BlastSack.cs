using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastSack : Enemy
{
    [SerializeField] public GameObject projectileType;
    private enum Phase {
        Wander = 1,
        Detonation = 2
    }
    private Phase curPhase;
    private float DetonationTime;

    void Start() {
        DetonationTime = Time.time + 5;
        curPhase = Phase.Wander;
    }

    void Update() {
        switch (curPhase) {

            case (Phase.Wander): {
                GameObject closestPlayer = FindClosestPlayer();
                if (closestPlayer != null) {
                    trackerController.SetTarget(closestPlayer.transform);
                }
                break;
            }

            case (Phase.Detonation): {
                FireInRings(projectileType, 8, 360/8, 0);
                Die();
                break;
            }
        }
    }

    public virtual void TriggerEvent(Collider2D other) {
        if (other.gameObject.tag == "player") {
            DetonationTime = Time.time + 0.5F;
            curPhase = Phase.Detonation;
        }
    }
}
