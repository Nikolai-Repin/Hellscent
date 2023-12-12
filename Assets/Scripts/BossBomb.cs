using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomb : Entity
{
    private float fuse = 3F;

    [SerializeField] public GameObject projectileType;
    public Boss creator;
    private Animator animator;
    [SerializeField] private BombType bombType;
    private enum BombType {
        Circle = 1,
        Line = 2,
    }
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
                    if (bombType == BombType.Circle) {
                        FireInRings(projectileType, 8, 360/8, 0, 2);
                    } else if (bombType == BombType.Line) {
                        SlowingLineShot(projectileType, 6, 15, 3, transform.rotation);
                        SlowingLineShot(projectileType, 6, 15, 3, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z+180));
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
