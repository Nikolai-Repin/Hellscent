using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] presets;
    [SerializeField] private int[] repeats;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private Color averageColor = new Vector4(0.8f, 0.8f, 0.8f, 1f);
    private int floor = 0;
    public bool hide = false;

    void Start() {
        floor = 0;  
        StartCoroutine(RunGame());
    }

    IEnumerator RunGame() {
        int repeatLevel = 0;
        for (int i = 0; i < repeats[repeats.Length - 1]; i++) { 
            if (i + 1 > repeats[repeatLevel]) {
                repeatLevel++;
            }
            player.position = new();
            hide = true;
            floor++;
            GameObject dungeon = Instantiate(presets[repeatLevel], new Vector2(), new Quaternion());
            GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = (dungeon.GetComponent<GenerateDungeon>().floorColor + averageColor)/2;
            while (dungeon.GetComponent<GenerateDungeon>().finished != true) {
                yield return null;
            }
            hide = false;
            while (!dungeon.GetComponent<GenerateDungeon>().dungeonOver) {
                yield return null;
            }   
            Destroy(dungeon);
        }
        endScreen.SetActive(true);
    }

    public int getFloor() {
        return floor;
    }
}
