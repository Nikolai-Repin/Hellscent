using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    [SerializeField] protected bool dealDamageOnContact;
    [SerializeField] protected bool holdsWeapon;
    [SerializeField] protected GameObject weapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    protected void OnTriggerEnter2D(Collider2D other) {
        if(dealDamageOnContact) {Debug.Log("Feature not implemented");}
    }
}
