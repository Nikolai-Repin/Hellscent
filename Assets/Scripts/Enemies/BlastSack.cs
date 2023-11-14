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
    private enum Phase {
        Wander = 1,
        Detonation = 2
    }
    private Phase curPhase;
    private float DetonationTime;

    void Start() {
        DetonationTime = Time.time + 5;
        curPhase = Phase.Wander;
        Register();
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
                FireInRings(projectileType, projectileCount, 360/projectileCount, rotationOffset, rings);
                Die();
                break;
            }
        }
    }

    public override void TriggerEvent(Collider2D other) {
        if (other.gameObject.tag == "player" &&  curPhase == Phase.Wander) {
            DetonationTime = Time.time + fuse;
            curPhase = Phase.Detonation;
        }
    }
}
