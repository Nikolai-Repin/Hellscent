using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSplit : Bullet
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private int projectileCount;
    [SerializeField] private float rotationAmount;
    [SerializeField] private float rotationOffset;
    [SerializeField] private int rings;

    public override void Die() {
        FireInRings(projectile, projectileCount, rotationAmount, rotationOffset, rings);
        Destroy(transform.gameObject);
    }
}
