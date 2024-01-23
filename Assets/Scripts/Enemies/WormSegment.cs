using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSegment : Enemy
{
    public WormBoss head;
    private Entity headScript;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetBool("Blue", head.blueMode);
        base.Start();
    }

    public void FireRing(GameObject projectile, int count, float offset, float speed) {
        CircleShot(projectile, count, offset, speed);
    }

    public override bool TakeDamage(float damage) {
        return head.TakeDamage(damage*0.75F);
    }

}
