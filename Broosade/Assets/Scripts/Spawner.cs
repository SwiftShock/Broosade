using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spawner : MonoBehaviour {

    public PlayerController playerScript;
    private GameObject enemyType;
    public float wait = 1f;
    public float counter = 1f;
    public int enemyCount;
    public int enemyLimit;
    public GameObject thisEnemy;

    void Awake()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        counter = 0;
    }

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (playerScript.hasDied || playerScript.meterChanged)
        {
            counter = 0;
            if(thisEnemy != null)
            {
                Destroy(thisEnemy);
            }
            StartCoroutine("RespawnEnemy");
        }
        else
        {
            SpawnEnemy();
        }
	}

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(playerScript.respawnDelay);
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (counter < wait)
        {
            if (playerScript.meter == 1)
            {
                thisEnemy = Instantiate(Resources.Load("Green Slime"), transform.position, transform.rotation) as GameObject;
            }
            else if (playerScript.meter == 2)
            {
                thisEnemy = Instantiate(Resources.Load("Yellow Slime"), transform.position, transform.rotation) as GameObject;
            }
            else if (playerScript.meter == 3)
            {
                thisEnemy = Instantiate(Resources.Load("Red Slime"), transform.position, transform.rotation) as GameObject;
            }
            counter += 1;
        }
    }
}
