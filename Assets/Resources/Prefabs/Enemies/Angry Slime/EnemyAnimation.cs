using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
             anim.Play("walkUp");
        if (Input.GetKeyDown(KeyCode.DownArrow))
             anim.Play("walkDown");
        if (Input.GetKeyDown(KeyCode.LeftArrow))
             anim.Play("walkLeft");
        if (Input.GetKeyDown(KeyCode.RightArrow))
             anim.Play("walkRight");
    }
}
