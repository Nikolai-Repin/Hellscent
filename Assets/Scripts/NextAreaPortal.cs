using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextAreaPortal : Entity
{
    private GenerateDungeon dungeon; 
    private GameObject[] dList;
    public bool active;
    private Animator animator;
    [SerializeField] private float activationDelay;
    private float timeTillActive;
    void Start() 
    {
        Register();
        active = false;
        timeTillActive = activationDelay;
        animator = gameObject.GetComponent<Animator>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
    }
    // Start is called before the first frame update
    void Update()
    {
        if(dungeon == null) 
        {
            dList = GameObject.FindGameObjectsWithTag("Dungeon");
            if (dList.Length > 0) {
                dungeon = dList[0].GetComponent<GenerateDungeon>();
            }
            
        } else 
        {
            timeTillActive -= Time.deltaTime;
            if (timeTillActive <= 0) 
            {
                active = true;
                animator.SetTrigger("Idling");
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D other) 
    {
        if(active && other.gameObject.tag == "player") 
        {
            other.gameObject.GetComponent<PlayerController>().RestoreHP(1);
            uiManager.updateHealth();
            active = false;
            dungeon.dungeonOver = true;
            Entity.ResetAll();
        }
    }
}
