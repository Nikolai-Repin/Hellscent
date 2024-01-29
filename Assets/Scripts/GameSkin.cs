using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSkin : MonoBehaviour
{
    public GameObject selectedskin;
    public GameObject Player;

    private Sprite playersprite;
    
    void Start()
    {
        //sets ingame skin to saved saved sprite prefab
        playersprite = selectedskin.GetComponent<SpriteRenderer>().sprite;
        //saves that sprite with the player
        Player.GetComponent<SpriteRenderer>().sprite = playersprite;
    }

    
}
