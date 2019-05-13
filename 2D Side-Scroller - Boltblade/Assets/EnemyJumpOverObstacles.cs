using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpOverObstacles : MonoBehaviour
{

    public float dirX;
    public float moveSpeed = 3f;
    public Rigidbody2D rb;
    public bool facingRight = false;
    public Vector3 localScale;
    public Transform target;
    public bool isGrounded;
    public float jumpForce;
    //public int health;
    //public GameObject blood;
    //private float dazedTime;
    //public float startDazedTime;

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < target.position.x - 0.5)
        {
            dirX = 1f;
        }
        else if (transform.position.x > target.position.x + 0.5)
        {
            dirX = -1f;
        }
    

    /*
        //If health reaches 0 the enemy dies
        if (health <= 0) {
            Destroy(gameObject);
        }

        //Enemy stops moving when hit
        if (dazedTime <= 0)
        {
            moveSpeed = 3f;
        }
        else {
            moveSpeed = 0;
            dazedTime -= Time.deltaTime;
        }

    */
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    private void LateUpdate()
    {
        CheckPosition();
    }

    void CheckPosition()
    {
        if (dirX > 0)
        {
            facingRight = true;
        }
        else if (dirX < 0)
        {
            facingRight = false;
        }
        if ((facingRight && localScale.x > 0) || (!facingRight && localScale.x < 0))
        {
            localScale.x *= -1;
        }

        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Ground":
                rb.AddForce(Vector2.up * jumpForce);

                break;
        }
    }

    /*
    //Enemy receives damage when the player attacks
    public void takeDamage(int damage) {
        dazedTime = startDazedTime;
        Instantiate(blood, transform.position, Quaternion.identity);
        health = health - damage;
    }
    */
}

