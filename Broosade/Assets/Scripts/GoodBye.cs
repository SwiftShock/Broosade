using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoodBye : MonoBehaviour {

    // Use this for initialization
    public string main;

    public void TheEnd()
    {
        SceneManager.LoadScene(main);
    }
}
