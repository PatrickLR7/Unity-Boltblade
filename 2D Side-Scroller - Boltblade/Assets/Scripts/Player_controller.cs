﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    public float speed = 25f;
    public float maxSpeed = 5f;
    private Rigidbody2D playerRB2D;
    public int jumpPower = 12;
    private bool jump;
    public float desiredx;
    private Animator anim;
    public bool grounded;
    public bool facingRight = true;


    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public Animator playerAnim;
    public float attackRange;
    public int damage;
    public ParticleSystem especialAttack;
    private float timeBtwEspecial;
    public float startTimeBtwEspecial;
    public float attackDuration;

    // Start is called before the first frame update
    void Start()
    {
        playerRB2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAnim = GetComponent<Animator>();
        especialAttack.Stop();
        timeBtwEspecial = 0;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", Mathf.Abs(playerRB2D.velocity.x));
        anim.SetBool("grounded", grounded);

        //Controls Player Attack
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey("q") || Input.GetMouseButtonDown(0))
            {
                playerAnim.SetTrigger("attack2");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if (enemiesToDamage[i].gameObject.name.Equals("EnemyCollider"))
                    {
                        enemiesToDamage[i].GetComponentInParent<EnemyDeath>().takeDamage(damage);
                    }
                    else
                    {
                        enemiesToDamage[i].GetComponent<EnemyDeath>().takeDamage(damage);
                    }
                }
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }

        //Special Attack

            if (timeBtwEspecial <= 0)
            {
                if (Input.GetKey("e"))
                {
                    attackDuration = 2;
                    especialAttack.Play();
                    timeBtwEspecial = startTimeBtwEspecial;
                }
                
            }
            else {
                timeBtwEspecial -= Time.deltaTime;
                attackDuration -= Time.deltaTime;
            }

            if (attackDuration <= 0) {
                especialAttack.Stop();
            }
            
        
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }




    //Should always be used for physics calculations.
    private void FixedUpdate()
    {
        //Controls Player Movement
        float h = Input.GetAxis("Horizontal");

        //playerRB2D.AddForce(Vector2.right * speed * h);
        playerRB2D.velocity = new Vector2(maxSpeed * h, playerRB2D.velocity.y);

        if (Input.GetButton("Jump") && grounded == true)
        {
            playerRB2D.velocity = new Vector2(0, 8);
            //playerRB2D.velocity.y = 6.5f;
        }

        /*
        if (playerRB2D.velocity.x > maxSpeed) {
            playerRB2D.velocity = new Vector2(maxSpeed, playerRB2D.velocity.y);   
        }
        else if (playerRB2D.velocity.x < -maxSpeed)
        {
            playerRB2D.velocity = new Vector2(-maxSpeed, playerRB2D.velocity.y);     
        }
        */
        float limitedSpeed = Mathf.Clamp(playerRB2D.velocity.x, -maxSpeed, maxSpeed);
        playerRB2D.velocity = new Vector2(limitedSpeed, playerRB2D.velocity.y);

        if (Input.GetKey("right") || Input.GetKey("d")) {
            if (h > 0.1f && !facingRight) {
                Flip(); //Rotate Right
            }
            
        }
        if (Input.GetKey("left") || Input.GetKey("a")) {
            if (h < 0.1f && facingRight) {
                Flip(); //Rotate Left
            }
        }

    }

    void Flip() {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up * 180);
    }
}
