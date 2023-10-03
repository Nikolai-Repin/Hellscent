using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour



{
    private float cooldown;
    private GameObject parent;
    private SpriteRenderer sr;
    private Controller controller;
    public int NextAttackTimer = 5;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        sr = GetComponent<SpriteRenderer>();
        controller = parent.GetComponent<Controller>();
        controller.heldWeapons.Add(GetComponent<Weapon>());
        controller.ChangeWeapon(controller.heldWeapons.Count-1);
    }

    // Update is called once per frame
    void Update()
    {
      transform.position = parent.transform.position;
       if(NextAttackTimer > 5){
        Attacking();
       NextAttackTimer = -1;
       }
       NextAttackTimer++;
    }
public void Attacking(){



}
}

