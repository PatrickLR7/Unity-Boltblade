using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{

    public float dirX;
    public float moveSpeed = 3f;
    public Rigidbody2D rb;
    public bool facingRight = false;
    public Vector3 localScale;
    public Transform target;
    public bool isGrounded;
    public float jumpForce;
    public float jumpTime;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        jumpTime = Time.time + UnityEngine.Random.Range( 1.0f, 2.0f );;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        jumpForce = 250f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < (target.position.x - 0.5))
        {
            dirX = 1f;
        } else if (transform.position.x > (target.position.x - 0.5))
        {
            dirX = -1f;
        }
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
        if(dirX > 0)
        {
            facingRight = true;
        } else if (dirX < 0)
        {
            facingRight = false;
        }
        if ((facingRight && localScale.x < 0) || (!facingRight && localScale.x > 0))
        {
            localScale.x *= -1;
        }
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Environment":
                if (IsGrounded())
                {
                    Debug.Log("collision with wall");
                    Jump();
                }
                break;
            case "Skeleton":
                if (IsGrounded())
                {
                    Debug.Log("collision with enemy");
                    Jump();
                }
                break;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Environment":
                if (IsGrounded())
                {
                    Debug.Log("collision with wall");
                    Jump();
                }
                break;
            case "Skeleton":
                if (IsGrounded())
                {
                    Debug.Log("collision with enemy");
                    Jump();
                }
                break;
        }
    }

    public void Jump(){
        if(Time.time > jumpTime){
            rb.AddForce(Vector2.up * jumpForce);
            jumpTime = Time.time + UnityEngine.Random.Range( 1.0f, 2.0f );
        }
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;
        
        Debug.DrawRay(position, direction, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }
}