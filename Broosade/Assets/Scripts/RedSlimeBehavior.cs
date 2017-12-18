using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSlimeBehavior : MonoBehaviour {

    // Use this for initialization
    private Animator redSlimeAnimator;
    private Rigidbody2D rigi;
    public GameObject player;
    public GameObject deathParticle;
    private CircleCollider2D col;
    public float playerRange;
    public GameObject redSlimeParticle;
    private Renderer rend;
    private BoxCollider2D dangerZone;
    private bool exploding;

    // Use this for initialization
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        redSlimeAnimator = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();
        dangerZone = GetComponent<BoxCollider2D>();
        rend = GetComponent<Renderer>();
        dangerZone.enabled = false;
        player = GameObject.FindWithTag("Player");
        exploding = false;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyDirection();
        if (((player.transform.position.x > transform.position.x && player.transform.position.x < transform.position.x + playerRange)
            || (player.transform.position.x < transform.position.x && player.transform.position.x > transform.position.x - playerRange))
             && ((player.transform.position.y > transform.position.y && player.transform.position.y < transform.position.y + playerRange)
            || (player.transform.position.y < transform.position.y && player.transform.position.y > transform.position.y - playerRange)))
        {
            redSlimeAnimator.SetInteger("State", 1);
            if (exploding)
            {
                //dangerZone.enabled = true;
                //Invoke("DoneFor", 0.5f);
                player.GetComponent<PlayerController>().urDead = true;
                exploding = false;
                Invoke("urDeadReset", 0.1f);
            }
            else
            {
                player.GetComponent<PlayerController>().urDead = false;
            }
        }
        if (player.GetComponent<PlayerController>().hasDied)
        {
            player.GetComponent<PlayerController>().urDead = false;
        }
     }

    void urDeadReset()
    {
        player.GetComponent<PlayerController>().urDead = false;
    }

    void Explode() { //called at end of explode animation
        exploding = true;
        Instantiate(redSlimeParticle, transform.position, transform.rotation);
    }

    void DoneFor()
    {
        dangerZone.enabled = false;
    }

    IEnumerator OhNo(float duration)
    {
        Debug.Log("Ya blew it");
        dangerZone.enabled = true;
        yield return new WaitForSeconds(0.5f);
        dangerZone.enabled = false;
    }

    void EnemyDirection()
    {
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector2(-1.0f, 1.0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Melee Attack" || other.tag == "Aerial Attack" || other.tag == "Bullet" || other.tag == "Dash Attack")
        {
            ScoreManager.score += 500;
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
        redSlimeAnimator.SetInteger("State", 2);
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
