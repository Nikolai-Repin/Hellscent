using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameManager : MonoBehaviour
{
    public static UIGameManager instance;

    public ItemManager itemManager;
    public UI_Manager uiManager;

    public Player player;

    private void Awake() {
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
        }

        else {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        itemManager = GetComponent<ItemManager>();
        uiManager = GetComponent<UI_Manager>();

        player = FindObjectOfType<Player>();
    }
}