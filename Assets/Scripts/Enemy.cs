using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    [SerializeField] protected bool dealDamageOnContact;
    [SerializeField] protected bool holdsWeapon;
    [SerializeField] protected GameObject weapon;
    [SerializeField] protected GameObject target;
    [SerializeField] protected float visRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) {
            weapon.GetComponent<Weapon>().Fire();
            weapon.GetComponent<Weapon>().SetTarget(target.transform.position);
        } else {
            target = FindClosestPlayer(visRange);
            Debug.Log(target.transform.position);
            weapon.GetComponent<Weapon>().SetTarget(target.transform.position);
        }
    }

    //
    protected void OnTriggerEnter2D(Collider2D other) {
        if (dealDamageOnContact) {Debug.Log("Feature not implemented");}
    }

    //Returns closest player in range
    public GameObject FindClosestPlayer(float range) {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, range);
        List<Collider2D> players = new List<Collider2D>();
        for (int i = 0; i < results.Length; i++) {
            if (results[i].transform.GetComponent<Controller>() != null) {
                players.Add(results[i]);
            }
        }
        if (results.Length > 0) {
            return FindClosest(players, transform.position);
        }
        return null;
    }

    public GameObject FindClosestPlayer() {
        return FindClosestPlayer(visRange);
    }
}
