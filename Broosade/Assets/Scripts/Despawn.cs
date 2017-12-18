using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour {

    public GameObject[] allEnemies;
    public PlayerController playerScript;

    // Use this for initialization
    void Start () {
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy Space");
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (playerScript.hasDied)
        {
            foreach (GameObject enemy in allEnemies)
            {
                Destroy(enemy);
            }
        }
	}
}
