using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormTurret : FlareSentry
{
    public WormBoss creator;

    // Start is called before the first frame update
    void Start()
    {
        rotationOffset = (int)(Random.Range(0, 24)*15);
        rotationSpeed *= (int)(Random.Range(0, 2)-1);
    }

    public override void Die() {
        creator.RemoveTurret(transform.gameObject);
        base.Die();
    }
}
