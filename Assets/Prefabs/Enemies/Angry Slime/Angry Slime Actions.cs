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
      if (getHealth() <= 0) {
                slimeDeath();
      }  
    }

    public void DeathAnim(){
        Anim.Play("slimeDeath");
    }

    public virtual void slimeDeath() {
        DeathAnim();
        Destroy(transform.gameObject);
    }
}
