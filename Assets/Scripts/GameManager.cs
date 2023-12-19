using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] presets;
    [SerializeField] private int[] repeats;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject endScreen;
    public bool hide = false;

    void Start() {
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
            GameObject dungeon = Instantiate(presets[repeatLevel], new Vector2(), new Quaternion());
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
}
