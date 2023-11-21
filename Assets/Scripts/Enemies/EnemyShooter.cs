using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyShooter : Enemy
{
    [SerializeField] protected GameObject weapon;
    [SerializeField] protected int clipSize;
    [SerializeField] protected float reloadTime;
    [SerializeField] private int ammo;
    private float reloadLastTime;
    protected GameObject target;

    void Start() {
        reloadLastTime = Time.time;
        ammo = clipSize;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        target = FindClosestPlayer(visRange);
        if (target != null) {
            if (ammo > 0 && Time.time > reloadLastTime) {
                if (weapon.GetComponent<Weapon>().Fire()) {ammo--;}
                weapon.GetComponent<Weapon>().SetTarget(target.transform.position);
            } else if (ammo == 0) {
                ammo = clipSize;
                reloadLastTime = Time.time + reloadTime;
            }
        }
    }

    public override void TriggerEvent(Collider2D other) {
        float forceMulti = 0.1f;

        Vector2 pushVector = (-1 * (other.transform.position - transform.position).normalized * forceMulti);
        GetComponent<AIBase>().velocity2D += pushVector;
    }
}
