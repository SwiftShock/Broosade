using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSProjectileFiring : MonoBehaviour
{

    public float speed = 5;
    public float height = 0;
    public Rigidbody2D rigiBullet;
    public GameObject player;
    public GameObject yellowSlime;
    public YellowSlimeBehavior yellowSlimeScript;

    // Use this for initialization
    void Start()
    {
        yellowSlimeScript = FindObjectOfType<YellowSlimeBehavior>();
        player = GameObject.FindWithTag("Player");
        rigiBullet = GetComponent<Rigidbody2D>();

        if (player.transform.position.x < transform.position.x)
        {
            speed = -5;
        }
        else
        {
            speed = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        rigiBullet.velocity = new Vector2 (speed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Checkpoint")
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground" || other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }   
    }
}