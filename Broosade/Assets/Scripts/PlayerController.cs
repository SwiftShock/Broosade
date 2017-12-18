using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //private stats
    public float movementSpeed;
    public float jumpHeight;
    public float jumpOrFall;
    public int attackDmg = 1;
    private float trueSpeed;

    //references
    private Rigidbody2D rigi;
    private Renderer rend;
    private Animator flintAnimator;
    public GameObject deathParticle;
    public GameObject respawnParticle;
    public GameObject bulletForward;
    public GameObject bulletNESW;
    public GameObject bulletSENW;
    private GameObject theBullet;
    private CircleCollider2D personalCollider;

    //booleans
    public bool isGrounded;
    public bool urDead;
    public bool facingRight;
    private bool isOnWall;
    private bool isWallClinging;
    private bool invincibility;
    private bool aerialAttacking;
    private bool canShoot;
    public bool aimingUp;
    public bool aimingForward;
    public bool aimingDown;
    private bool meleeAttacking;
    private bool dashAttacking;
    private bool isShooting;
    private bool isDashing;
    private bool isWalking;
    public bool hasDied;

    //colliders and transforms
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask wallLayer;
    public Vector3 respawnPoint;
    public Collider2D meleeAttackTrigger;
    public Collider2D dashAttackTrigger;
    public Collider2D aerialAttackTrigger;
    public Transform firePoint;

    //hud stats
    public int maxHealth;
    public int health;
    private int totalHealth = 5;
    public int meter;
    private int totalMeter = 3;
    public int numOfDeaths;
    public int enemiesKilled;
    public bool hi = false;
    private int prevNumOfDeaths;
    
    
    //timers
    public float respawnDelay;
    private float attackTimer = 0;
    private float attackFrame = 0.35f;

    //knockback
    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public Vector2 knockBackDirection;
    public bool meterChanged;
    public bool gottem;

    // Use this for initialization
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();
        personalCollider = GetComponent<CircleCollider2D>();
        flintAnimator = GetComponent<Animator>();
        hi = true;
        facingRight = true;
        isGrounded = true;
        canShoot = true;
        isWallClinging = false;
        invincibility = false;
        aerialAttacking = false;
        meleeAttacking = false;
        dashAttacking = false;
        aimingUp = false;
        aimingForward = false;
        aimingDown = false;
        maxHealth = PlayerPrefs.GetInt("MaxHealth", 2);
        health = PlayerPrefs.GetInt("Health", 2);
        meter = PlayerPrefs.GetInt("Meter", 1);
        numOfDeaths = PlayerPrefs.GetInt("NumOfDeaths", 0);
        prevNumOfDeaths = numOfDeaths;
        enemiesKilled = PlayerPrefs.GetInt("EnemiesKilled", 0);
        canShoot = true;
        respawnDelay = 1;
        meleeAttackTrigger.enabled = false;
        dashAttackTrigger.enabled = false;
        aerialAttackTrigger.enabled = false;
        attackTimer = attackFrame;
        isDashing = false;
        isWalking = false;
        hasDied = false;
        meterChanged = false;
        gottem = false;
        urDead = false;
    }

	// Update is called once per frame
	void Update () {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isOnWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
        Direction();
        WalkAndIdle();
        JumpAndFall();
        WallClingAndJump();
        HealthStuff();
        AggressionMeterStuff();

        if(rigi.velocity.x == 0)
        {
            trueSpeed = 1;
            isWalking = false;
            isDashing = false;
        }

        if (canShoot)
        {
            Aim();
        }

        if (isGrounded)
        {
            aerialAttacking = false;
            canShoot = true;
        }

        if(!aerialAttacking && rigi.velocity.y < 0)
        {
            aerialAttackTrigger.enabled = false;
        }

        if (!isGrounded || isWallClinging)
        {
            canShoot = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) { //jump
            rigi.velocity = new Vector2(rigi.velocity.x, jumpHeight);
            canShoot = false;
        }

        if(Input.GetKeyDown(KeyCode.Space) && !aerialAttacking && !isGrounded)//double jump-aerial attack
        {
            rigi.velocity = new Vector2(0, jumpHeight * 0.7f);
            aerialAttacking = true;
            aerialAttackTrigger.enabled = true;
        }

            if (Input.GetKey(KeyCode.D)) //move right
            {
                canShoot = false;
                if (Input.GetKey(KeyCode.F) && !dashAttacking)//dash attack right
                {
                    isDashing = true;
                    trueSpeed = movementSpeed * 2;
                    rigi.velocity = new Vector2(trueSpeed, rigi.velocity.y);
                    dashAttacking = true;
                    dashAttackTrigger.enabled = true;
                }
                else
                {
                    isWalking = true;
                    trueSpeed = movementSpeed;
                    rigi.velocity = new Vector2(trueSpeed, rigi.velocity.y);
                    dashAttacking = false;
                    dashAttackTrigger.enabled = false;
                    
                }
            }

            if (Input.GetKey(KeyCode.A)) //move left
            {
                canShoot = false;
                if (Input.GetKey(KeyCode.F) && !dashAttacking)//dash left
                {
                    isDashing = true;
                    trueSpeed = -movementSpeed * 2;
                    rigi.velocity = new Vector2(trueSpeed, rigi.velocity.y);
                    dashAttacking = true;
                    dashAttackTrigger.enabled = true;
            }
                else
                {
                    isWalking = true;
                    trueSpeed = -movementSpeed;
                    rigi.velocity = new Vector2(trueSpeed, rigi.velocity.y);
                    dashAttacking = false;
                    dashAttackTrigger.enabled = false;
                }
            }

        if (Input.GetKeyDown(KeyCode.V) && !isWallClinging && !meleeAttacking) //melee attack
        {
            canShoot = false;
            meleeAttacking = true;
            meleeAttackTrigger.enabled = true;
            flintAnimator.SetInteger("State", 5);
        }

        if (meleeAttacking)
        {
            if(attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                meleeAttacking = false;
                meleeAttackTrigger.enabled = false;
                attackTimer = attackFrame;
            }
        }

        if (urDead)
        {
            health -= 1;
            if (health != 0)
            {
                StartCoroutine(Knockback(0.1f, 30, trueSpeed));
                //StartCoroutine("CollideFlash");
            }
            invincibility = true;
            Invoke("resetInvulnerability", 1);
            urDead = false;
        }
    }

    void HealthStuff()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
            PlayerPrefs.SetInt("Health", health);
        }

        if (maxHealth > totalHealth)
        {
            maxHealth = totalHealth;
            health = maxHealth;
            PlayerPrefs.SetInt("Health", health);
        }

        if (health <= maxHealth)
        {
            PlayerPrefs.SetInt("Health", health);
        }

        if (maxHealth <= totalHealth)
        {
            PlayerPrefs.SetInt("MaxHealth", maxHealth);
        }

        if (health < 0)
        {
            health = 0;
        }

        if (health == 0)
        {
            health = 0;
            numOfDeaths++;
            PlayerPrefs.SetInt("NumOfDeaths", numOfDeaths);
            StartCoroutine("RespawnPlayer");
        }
    }

    void AggressionMeterStuff()
    {
        //all addition to enemies killed occurs in the enemy script

        if (enemiesKilled >= 5 && enemiesKilled % 5 == 0)
        {
            meter++;
            meterChanged = true;
            Invoke("EnemySpawnerStuff", 1);
            PlayerPrefs.SetInt("Meter", meter);
            enemiesKilled = 0;
            PlayerPrefs.SetInt("EnemiesKilled", enemiesKilled);
        }

        if(numOfDeaths >= 3 && numOfDeaths % 3 == 0)
        {
            meter--;
            meterChanged = true;
            Invoke("EnemySpawnerStuff", 1);
            PlayerPrefs.SetInt("Meter", meter);
            numOfDeaths = 0;
            PlayerPrefs.SetInt("NumOfDeaths", numOfDeaths);
        }

        if(meter >= totalMeter)
        {
            meter = totalMeter;
            enemiesKilled = 0;
            PlayerPrefs.SetInt("EnemiesKilled", enemiesKilled);
        }

        if(meter < 1)
        {
            meter = 1;
            numOfDeaths = 0;
            PlayerPrefs.SetInt("NumOfDeaths", numOfDeaths);
        }

        PlayerPrefs.SetInt("Meter", meter);

    }

    void Direction()
    {
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            facingRight = true;
            transform.localScale = new Vector2(1.0f, 1.0f);
        }
        else if (Input.GetAxis("Horizontal") < -0.1f)
        {
            facingRight = false;
            transform.localScale = new Vector2(-1.0f, 1.0f);
        }
    }

    void WalkAndIdle()
    {
        if (isGrounded && !isWallClinging && rigi.velocity.x!=0)//walking animation
        {
            flintAnimator.SetInteger("State", 2);
            if (Input.GetKey(KeyCode.F) && !isWallClinging)
            {
                flintAnimator.SetInteger("State", 7);
            }
        }
        else
        {
            flintAnimator.SetInteger("State", 0);
        }
    }

    void JumpAndFall()
    {
        if (!isGrounded)
        {
            if (rigi.velocity.y > 0) //jumping animation
            {
                if (aerialAttacking)
                {
                    flintAnimator.SetInteger("State", 8);
                }
                else
                {
                    flintAnimator.SetInteger("State", 3);
                }
            }
            else //falling animation
            {
                flintAnimator.SetInteger("State", 1);
            }
        }
    }

    void Aim()
    {
        if (Input.GetKey(KeyCode.H))
        {
             rigi.constraints = RigidbodyConstraints2D.FreezeAll;
             if (Input.GetKey(KeyCode.U))
                {
                    aimingUp = true;
                    aimingDown = false;
                    aimingForward = false;
                    flintAnimator.SetInteger("State", 11);
                }
                else if (Input.GetKey(KeyCode.N))
                {
                    aimingDown = true;
                    aimingUp = false;
                    aimingForward = false;
                    flintAnimator.SetInteger("State", 12); 
                }
                else
                {
                    aimingForward = true;
                    aimingDown = false;
                    aimingUp = false;
                    flintAnimator.SetInteger("State", 10);
                }
            }
        else if (Input.GetKeyUp(KeyCode.H))
        {
            Shoot();
        }
    }

    void Shoot(){
        setRightBulletObject();
        if (aimingUp)
            {
                flintAnimator.SetInteger("State", 14);
            }
        else if (aimingForward)
            {
                flintAnimator.SetInteger("State", 13);
            }
        else if (aimingDown)
            {
                flintAnimator.SetInteger("State", 15);
            }
        Instantiate(theBullet, firePoint.position, firePoint.rotation);
        if (!isWallClinging)
        {
            rigi.constraints = RigidbodyConstraints2D.None;
            rigi.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
     }

    void setRightBulletObject()
    {
        if (facingRight && aimingDown || !facingRight && aimingUp)
        {
            theBullet = bulletSENW;
        }
        else if (facingRight && aimingUp || !facingRight && aimingDown)
        {
            theBullet = bulletNESW;
        }
        else if (aimingForward)
        {
            theBullet = bulletForward;
        }
    }

    void WallClingAndJump()
    {
        if (isOnWall && !isGrounded)
        {
            if((facingRight && Input.GetKey(KeyCode.D)) || (!facingRight && Input.GetKey(KeyCode.A))){
                rigi.constraints = RigidbodyConstraints2D.FreezeAll;
                isWallClinging = true;
            }
        }
        if (isWallClinging)
        {
            flintAnimator.SetInteger("State", 4);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isWallClinging = false;
                rigi.velocity = new Vector2(rigi.velocity.x, jumpHeight);
                JumpAndFall();
                rigi.constraints = RigidbodyConstraints2D.None;
                rigi.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "Bottom Boundary")
        {
            health = 0;
            StartCoroutine("RespawnPlayer");
        }
        if(other.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
        if (other.tag == "Heart Container")
        {
            maxHealth++;
            health = maxHealth;
            ScoreManager.score += 10000;
            PlayerPrefs.SetInt("Score", ScoreManager.score);
            Instantiate(respawnParticle, transform.position, transform.rotation);
            Destroy(other.gameObject);
        }
        if (!invincibility)
        {
            if (other.tag == "Danger Zone")
            {
                health -= 3;
                if (health != 0)
                {
                    StartCoroutine(Knockback(0.1f, 30, trueSpeed));
                    //StartCoroutine("CollideFlash");
                }
                invincibility = true;
                Invoke("resetInvulnerability", 1);
            }
        }      
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!invincibility)
        {
            if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Enemy Bullet")
            {
                health -= 1;
                if (health != 0)
                {
                    StartCoroutine(Knockback(0.1f, 30, trueSpeed));
                    //StartCoroutine("CollideFlash");
                }
                invincibility = true;
                Invoke("resetInvulnerability", 1);
            }
        }
    }

    public IEnumerator RespawnPlayer()
    {
        Instantiate(deathParticle, transform.position, transform.rotation);
        enabled = false;
        rend.enabled = false;
        hasDied = true;
        Invoke("EnemySpawnerStuff", 1);
        yield return new WaitForSeconds(respawnDelay);
        transform.position = respawnPoint;
        enabled = true;
        rend.enabled = true;
        health = maxHealth;
        Instantiate(respawnParticle, transform.position, transform.rotation);
        invincibility = true;
        Invoke("resetInvulnerability", 1);
    }

    void EnemySpawnerStuff() {
        hasDied = false;
        meterChanged = false;
    }

    void resetInvulnerability()
    {
        invincibility = false;
        hasDied = false;
    }
    
    public IEnumerator Knockback(float kBDuration, float kBPower, float kBDirection)
    {
        float timer = 0;
        Vector2 playerPosition = new Vector2 (transform.position.x - 1, transform.position.y + 1);
        while(kBDuration > timer)
        {
            flintAnimator.SetInteger("State", 20);
            timer += Time.deltaTime;
            rigi.velocity = new Vector2(kBDirection * -5, kBPower);
        }
        yield return 0;
    }

    IEnumerator CollideFlash()
    {
        Material m = this.rend.material;
        Color32 c = this.rend.material.color;
        this.rend.material = null;
        this.rend.material.SetColor("_Color", Color.white);
        yield return new WaitForSeconds(1f);
        this.rend.material = m;
        this.rend.material.color = c;
    }
}
