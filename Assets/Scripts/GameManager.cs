using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] presets;
    [SerializeField] private int[] repeats;

    void Start() {
        StartCoroutine(RunGame());
    }

    IEnumerator RunGame() {
        int repeatLevel = 0;
        for (int i = 0; i < repeats[repeats.Length - 1]; i++) { 
            if (i + 1 > repeats[repeatLevel]) {
                repeatLevel++;
            }
            GameObject dungeon = Instantiate(presets[repeatLevel], new Vector2(), new Quaternion());
            while (!dungeon.GetComponent<GenerateDungeon>().dungeonOver) {
                yield return null;
            }
            Destroy(dungeon);
        }
    }
}
