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
    [SerializeField] private bool moveWhileShooting;
    [SerializeField] private ShooterType sType;
    private enum ShooterType
    {
        Shooter = 0,
        Popper = 1,
    }
    private static int shooterCount = 0;
    private int shooterIndex;
    private float reloadLastTime;
    protected GameObject target;
    public enum Phase
    {
        Aiming = 1,
        Firing = 2,
    }
    private Phase curPhase;

    void Start() {
        base.Start();
        if (sType == ShooterType.Shooter) {

            shooterIndex = shooterCount;
            shooterCount++;
        }
        
        curPhase = Phase.Aiming;
        reloadLastTime = Time.time;
        ammo = clipSize;
    }

    // Update is called once per frame
    new void Update()
    {

        base.Update();
        if (target != null) {
            if (ammo > 0 && Time.time > reloadLastTime && (target.transform.position - transform.position).sqrMagnitude <= 2500) {
                if (weapon.GetComponent<Weapon>().Fire()) {
                    ammo--;
                    curPhase = Phase.Firing;
                    if (!moveWhileShooting) {
                        GetComponent<AIBase>().canMove = false;
                    }
                }                
            } else if (ammo == 0) {
                target = FindClosestPlayer(visRange);
                GetComponent<AIBase>().canMove = true;
                curPhase = Phase.Aiming;
                ammo = clipSize;
                reloadLastTime = Time.time + Random.Range(reloadTime, reloadTime+(reloadTime/3));

                if (sType == ShooterType.Shooter) { 
                    Debug.Log(shooterCount);
                    if (shooterCount <= 0) {
                        shooterCount += 1; //This may cause some unintended side effects, but this should counterract a divide by zero error from the modulo
                    }
                    reloadLastTime += (shooterIndex%shooterCount)*0.5F;
                }
            }

            if (curPhase != Phase.Firing || moveWhileShooting) {
                weapon.GetComponent<Weapon>().SetTarget(target.transform.position);
            }
        } else {
            target = FindClosestPlayer(visRange);
            ammo = 0;
        }
    }

    public override void Die() {
        switch (sType) {
            case ShooterType.Shooter: {
                shooterCount--;
                break;
            }
        }
        
        base.Die();
    }
}