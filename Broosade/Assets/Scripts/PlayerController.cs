using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float movementSpeed;
    public float jumpHeight;
    private Rigidbody2D rigi;
    public bool isGrounded = false;
    public Transform groundCheck1;
    public float groundCheckRadius;
    public LayerMask groundLayer;

	// Use this for initialization
	void Start () {
        rigi = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck1.position, groundCheckRadius, groundLayer);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) { //jump
            rigi.velocity = new Vector2(rigi.velocity.x,jumpHeight);
        }

        if (Input.GetKey(KeyCode.D)) //move right
        {
            rigi.velocity = new Vector2(movementSpeed, rigi.velocity.y);
        }

        if (Input.GetKey(KeyCode.A)) //move left
        {
            rigi.velocity = new Vector2(-movementSpeed, rigi.velocity.y);
        }
    }
}
