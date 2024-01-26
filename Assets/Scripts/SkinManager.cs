using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SkinManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public List<Sprite> skins = new List<Sprite>();
    private int selectedSkin = 0;
    public GameObject playerskin;

    public void NextOption() {
        //goes to next skin in list
        selectedSkin++;
        //cycles back around if reached last skin
        if (selectedSkin == skins.Count) {
            selectedSkin = 0;
        }
        //sets skin to chosen skin
        sr.sprite = skins[selectedSkin];
    }

    public void BackOption() {
        //goes to previous skin in list
        selectedSkin--;
        //cycles to back if past first skin
        if (selectedSkin<0) {
            selectedSkin = skins.Count -1;
        }
        //sets skin to chosen skin
        sr.sprite = skins[selectedSkin];
    }

    public void PlayGame() {
        //saves skin as prefab to use in game
        PrefabUtility.SaveAsPrefabAsset(playerskin, "Assets/Resources/Prefabs/Player/selectedskin.prefab");
        //loads game
        SceneManager.LoadScene("Game");
    }
}
