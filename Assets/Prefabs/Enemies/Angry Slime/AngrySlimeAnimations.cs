using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyAnimation : Entity
{
    // Start is called before the first frame update
    public Animator anim;
    [SerializeField] private Transform player;
    public Component aiScript;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool notdead = true;
    Vector3 direction = player.position - transform.position;
    while (notdead){
        if(Input.GetKeyDown("space")){
            subHealth(10);
        }
    if (System.Math.Abs(direction.x) > System.Math.Abs(direction.y)){
     
     if(direction.x > 0){
     anim.Play("walkRight");
     }
     else{
     anim.Play("walkLeft");
     }
    }
     else{
        if(direction.y > 0){
     anim.Play("walkUp");
}   else{
    anim.Play("walkDown");
}
     }
     if(getHealth() <= 0){
        notdead = false;
        DeathAnim();
    }
if(Input.GetKeyDown("space")){
            subHealth(10);
        }
    }

    }

    
    public bool DeathAnim(){
        anim.Play("slimeDeath");
        Debug.Log("finished animation");
        return true;
    }
    public void isDead(){
        if (getHealth() <= 0){
            Destroy(gameObject.GetComponent<AIPath>());
        }
    }    

    }



