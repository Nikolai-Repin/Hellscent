using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy
{
    [SerializeField] protected GameObject weapon;
    protected GameObject target;

    // Update is called once per frame
    void Update()
    {
        target = FindClosestPlayer(visRange);
        if (target != null) {
            weapon.GetComponent<Weapon>().Fire();
            weapon.GetComponent<Weapon>().SetTarget(target.transform.position);
        }
    }
}
