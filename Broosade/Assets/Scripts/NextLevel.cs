using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {

    public string nextLevel;
    private bool canEnter = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canEnter = true;
             
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && canEnter)
        {
            SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
        }
    }
}
