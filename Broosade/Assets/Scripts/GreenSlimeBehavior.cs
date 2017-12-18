using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSlimeBehavior : MonoBehaviour {

    private Animator greenSlimeAnimator;
    private Rigidbody2D rigi;
    public float eMoveSpeed = 3;
    public bool eMoveRight;
    public float distance = 5;
    public float xStartPosition;
    public GameObject player;
    public GameObject deathParticle;
    private BoxCollider2D col;

    // Use this for initialization
    void Start () {
        rigi = GetComponent<Rigidbody2D>();
        greenSlimeAnimator = GetComponent<Animator>();
        eMoveRight = false;
        xStartPosition = transform.position.x;
        col = GetComponent<BoxCollider2D>();
        player = GameObject.FindWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        EnemyDirection();

        if((eMoveSpeed < 0 && transform.position.x < xStartPosition) || (eMoveSpeed > 0 && transform.position.x > xStartPosition + distance))
        {
            eMoveSpeed *= -1;
        }

        transform.position = new Vector2(transform.position.x + eMoveSpeed * Time.deltaTime, transform.position.y);
	}

    void EnemyDirection()
    {
        if (eMoveSpeed < 0)
        {
            transform.localScale = new Vector2(1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector2(-1.0f, 1.0f);
        }
    }

    void OnCollisionEnter2D(Collision2D cool)
    {
        if(cool.gameObject.tag == "Wall" || cool.gameObject.tag == "Enemy")
        {
            eMoveSpeed *= -1;
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "Melee Attack" || other.tag == "Dash Attack" || other.tag == "Aerial Attack" || other.tag == "Bullet")
        {
            ScoreManager.score += 100;
            PlayerPrefs.SetInt("Score", ScoreManager.score);
            player.GetComponent<PlayerController>().enemiesKilled++;
            PlayerPrefs.SetInt("EnemiesKilled", player.GetComponent<PlayerController>().enemiesKilled);
            StartCoroutine("Dead");
        }
    }

    public IEnumerator Dead()
    {
        float knockBackValue;
        Destroy(col);
        greenSlimeAnimator.SetInteger("State", 1);
        if (player.GetComponent<PlayerController>().facingRight)
        {
            knockBackValue = 15;
        }
        else
        {
            knockBackValue = -15;
        }
        rigi.velocity = new Vector2(knockBackValue, 15);
        yield return new WaitForSeconds(0.3f);
        Instantiate(deathParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
