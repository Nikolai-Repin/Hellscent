using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastSack : Enemy
{
    [SerializeField] private float fuse;
    [SerializeField] public GameObject projectileType;
    [SerializeField] private int projectileCount;
    [SerializeField] private float rotationOffset;
    [SerializeField] private int rings;
    private Animator animator;
    private enum Phase {
        Wander = 1,
        Detonation = 2
    }
    private Phase curPhase;
    private float DetonationTime;

    void Start() {
        DetonationTime = Time.time + 5;
        curPhase = Phase.Wander;
        animator = GetComponent<Animator>();
        base.Start();
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
                if(Time.time >= DetonationTime - 0.5) {
                    animator.SetBool("collapsing", true);
                }
                if(Time.time >= DetonationTime) {
                    FireInRings(projectileType, projectileCount, 360/projectileCount, rotationOffset, rings);
                    Die();
                }
                break;
            }
        }
    }

    public override void TriggerEvent(Collider2D other) {
        if (other.gameObject.tag == "player" &&  curPhase == Phase.Wander) {
            DetonationTime = Time.time + fuse;
            trackerController.aiPath.maxAcceleration /= 2;
            curPhase = Phase.Detonation;
        }
    }
}
