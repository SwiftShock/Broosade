using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFireHandler : MonoBehaviour {

    public float speed = 5;
    public float height = 5;
    private Rigidbody2D rigi;
    public PlayerController player;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerController>();
        rigi = GetComponent<Rigidbody2D>();

        if(!player.facingRight)
        {
            speed = -speed;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (player.aimingUp)
        {
            height = 5f;
        }
        else if (player.aimingDown)
        {
            height = -5f;
        }
        else
        {
            height = 0f;
        }
        rigi.velocity = new Vector2(speed, height);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "Checkpoint")
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }
}
