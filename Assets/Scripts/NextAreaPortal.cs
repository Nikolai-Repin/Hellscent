using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextAreaPortal : MonoBehaviour
{
    private GenerateDungeon dungeon; 
    // Start is called before the first frame update
    void Update()
    {
        if(dungeon == null) {
            dList = GameObject.FindGameObjectsWithTag("Dungeon");
            if (dList.Count > 0) {
                dungeon = dList[0].GetComponent<GenerateDungeon>();
            }
            
        }
    }

    protected void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "player") {
            dungeon.dungeonOver = true;
            Entity.ResetAll();
        }
    }
}
