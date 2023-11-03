using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    [SerializeField] private Transform player;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    Vector3 direction = player.position - transform.position;
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
    

    }


    }


