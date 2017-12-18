using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LetsGo : MonoBehaviour {

    public string howToPlay;

    public void HowToPlay()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(howToPlay);
    }
}
