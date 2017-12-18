using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string tutorial;

    public string mainGame;

    public void Tutorial()
    {
        SceneManager.LoadScene(tutorial);
    }

    public void MainGame()
    {
        SceneManager.LoadScene(mainGame);
    }
}
