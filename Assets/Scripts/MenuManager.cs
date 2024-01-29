using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int gameStartScene;

    public void QuitGame() {
        //closes running game
        Application.Quit();
    }
    
    public void StartGame()
    {
        //changes to main game scene
        SceneManager.LoadScene(gameStartScene);
    }
}
