using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngrySlimeActions : Entity
{
    public Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space")){
            subHealth(10);
        }

        
      if (getHealth() <= 0) {
                slimeDeath();
      }  
    }

    public bool DeathAnim(){
        Anim.Play("slimeDeath");
        Debug.Log("finished animation");
        return true;
    }

    public virtual void slimeDeath() {
        DeathAnim();
        Debug.Log("die");
       // Die();
    }
}
