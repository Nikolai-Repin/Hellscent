using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareSentry : Enemy
{

    [SerializeField] public GameObject projectileType;
    [SerializeField] private int projectileCount;
    [SerializeField] private float projectileDelay;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float rotationOffset;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int burstLength;
    [SerializeField] private float burstDelay;

    [SerializeField] private bool alternateDirections;
    private int firedShots;
    private float nextFireTime;

    void Start() {
        firedShots = 0;
        nextFireTime = Time.time;
        Register();
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
        CircleShot(projectileType, projectileCount*2, 0, projectileSpeed);
        base.Die();
    }
}
