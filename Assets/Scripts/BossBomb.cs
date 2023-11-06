using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomb : Entity
{
    private float fuse = 3F;

    [SerializeField] public GameObject projectileType;
    private Animator animator;
    private enum Phase {
        Charging = 1,
        Blasting = 2,
    }
    private Phase curPhase;

    // Start is called before the first frame update
    void Start()
    {
        curPhase = Phase.Charging;
        animator = GetComponent<Animator>();
        animator.SetFloat("Fuse", fuse);
    }

    // Update is called once per frame
    void Update()
    {
        fuse -= Time.deltaTime;
        switch (curPhase) {
            
            //Pre-Detonation
            case Phase.Charging: {
                if (fuse <= 0) {
                    int projectiles = 8; //How many projectiles in each ring
                    float rotationAmount = 360/projectiles;
                    float rotationOffset = 0;

                    //Outer for loop controls how many rings of projectiles
                    for (int k = 1; k <= 2; k++) {
                        //Inner for loop controls how many projectiles in each ring
                        for (int i = 0; i < projectiles; i++) {
                            GameObject bullet = Instantiate(projectileType, transform.position, new Quaternion());
                            Bullet bulletScript = bullet.GetComponent<Bullet>();
                            bulletScript.team = "Enemy";
                            Quaternion fireAngle = Quaternion.Euler(new Vector3(0, 0, (rotationAmount*i)+rotationOffset));
                            bulletScript.LaunchProjectile(fireAngle, 10/k);
                        }
                        rotationOffset += rotationAmount/2;
                    }
                    curPhase = Phase.Blasting;
                }
                break;
            }

            //Detonation
            case Phase.Blasting: {
                if (fuse <= -0.5) {
                    Die();
                }
                break;
            }
        }

        animator.SetFloat("Fuse", fuse);
    }
}
