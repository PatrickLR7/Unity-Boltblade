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
    public LayerMask groundLayer;

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
        } else if (transform.position.x > target.position.x + 0.5)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Environment":
                if (IsGrounded())
                {
                    rb.AddForce(Vector2.up * jumpForce);
                }
                break;
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

