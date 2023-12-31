using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;
public class EnemyAnimation : Entity {
  // Start is called before the first frame update
  public Animator anim;
  [SerializeField] private Transform player;
  void Start() {
    anim = gameObject.GetComponent <Animator>();
  }

  // Update is called once per frame
  void Update() {
    bool notdead = true;

    Vector3 direction = player.position - transform.position;

    if (System.Math.Abs(direction.x) > System.Math.Abs(direction.y)) {

      if (direction.x > 0) {
        anim.Play("walkRight");
      } else {
        anim.Play("walkLeft");
      }
    } else {
      if (direction.y > 0) {
        anim.Play("walkUp");
      } else {
        anim.Play("walkDown");
      }
    }
    if (Input.GetKeyDown("space")) {
      TakeDamage(10);

    }
    if (GetHealthAmount() <= 0) {
      var ai = GetComponent <AIPath> ();
      ai.canMove = false;

      enabled = false;
      DeathAnim();
      Destroy(gameObject, (float) .55);

    }

  }

  public bool DeathAnim() {
    anim.Play("slimeDeath");
    Debug.Log("finished animation");

    return true;
  }
}