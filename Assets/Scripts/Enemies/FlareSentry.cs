using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareSentry : Enemy
{

    [SerializeField] public GameObject projectileType;
    [SerializeField] private int projectileCount;
    [SerializeField] private float projectileDelay;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float rotationOffset;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] private int burstLength;
    [SerializeField] private float burstDelay;
    [SerializeField] private bool detonateOnDeath;

    [SerializeField] private bool alternateDirections;
    private int firedShots;
    private float nextFireTime;

    void Start() {
        firedShots = 0;
        nextFireTime = Time.time + 1;
        base.Start();
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextFireTime) {
            if (firedShots < burstLength) {
                CircleShot(projectileType, projectileCount, rotationOffset, projectileSpeed);
                nextFireTime = Time.time + projectileDelay;
                rotationOffset += rotationSpeed;
                firedShots++;
            } else {
                if (alternateDirections) {
                    rotationSpeed *= -1;
                }
                firedShots = 0;
                nextFireTime = Time.time + burstDelay;
            }
            
        }
    }

    public override void Die() {
        dealDamageOnContact = false;
        intangible = true;
        if (detonateOnDeath) {
            CircleShot(projectileType, projectileCount*2, 0, projectileSpeed);
        }
        
        base.Die();
    }

    public override void LastEntityEvent() {
        if (rotationSpeed == 0) {
            rotationSpeed = 10;
            projectileDelay = 0.5F;
        }
        if (Random.Range(0, 101) <= 10) {
            Instantiate(Resources.Load<GameObject>("Prefabs/Items/HealItem"), transform);
        }
        return;
    }
}
