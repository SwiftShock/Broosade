using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSlimeBehavior : MonoBehaviour {
    private Animator yellowSlimeAnimator;
    private Rigidbody2D rigi;
    public GameObject player;
    public GameObject deathParticle;
    private BoxCollider2D col;
    public GameObject projectile;
    public bool ySFacingRight;
    public Transform firePoint;
    private Rigidbody2D rigiBullet;
    public bool canShoot;
    public float playerRange = 3f;
    public float waitBetweenShots = 1f;
    private float shotCounter;

    // Use this for initialization
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        yellowSlimeAnimator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        rigiBullet = projectile.GetComponent<Rigidbody2D>();
        shotCounter = waitBetweenShots;
        player = GameObject.FindWithTag("Player");
        ySFacingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyDirection();
        shotCounter -= Time.deltaTime;
        if (shotCounter < 0 && (((player.transform.position.x > transform.position.x && player.transform.position.x < transform.position.x + playerRange)
            || (player.transform.position.x < transform.position.x && player.transform.position.x > transform.position.x - playerRange))
             && ((player.transform.position.y > transform.position.y && player.transform.position.y < transform.position.y + playerRange)
            || (player.transform.position.y < transform.position.y && player.transform.position.y > transform.position.y - playerRange))))
            {
            yellowSlimeAnimator.SetInteger("State", 1);
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(projectile, firePoint.position, firePoint.rotation);
        shotCounter = waitBetweenShots;
    }

    void EnemyDirection()
    {
        if (player.transform.position.x < transform.position.x)
        {
            ySFacingRight = false;
            transform.localScale = new Vector2(1.0f, 1.0f);
        }
        else if (player.transform.position.x > transform.position.x)
        {
            ySFacingRight = true;
            transform.localScale = new Vector2(-1.0f, 1.0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Melee Attack" || other.tag == "Dash Attack" || other.tag == "Aerial Attack" || other.tag == "Bullet")
        {
            ScoreManager.score += 300;
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
        yellowSlimeAnimator.SetInteger("State", 2);
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