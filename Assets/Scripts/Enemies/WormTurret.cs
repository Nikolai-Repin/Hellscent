using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormTurret : FlareSentry
{
    public WormBoss creator;

    public override void Die() {
        creator.RemoveTurret(transform.gameObject);
        base.Die();
    }
}
