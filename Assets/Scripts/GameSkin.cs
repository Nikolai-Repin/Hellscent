using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSkin : MonoBehaviour
{
    public GameObject selectedskin;
    public GameObject Player;

    private Sprite playersprite;
    // Start is called before the first frame update
    void Start()
    {
        playersprite = selectedskin.GetComponent<SpriteRenderer>().sprite;

        Player.GetComponent<SpriteRenderer>().sprite = playersprite;
    }

    
}
