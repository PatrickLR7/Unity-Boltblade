using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float dirX;
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public Vector3 localScale;
    public Transform target;
    public bool facingRight = false;

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
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

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
        if ((facingRight && localScale.x < 0) || (!facingRight && localScale.x > 0))
        {
            localScale.x *= -1;
        }

        transform.localScale = localScale;
    }
}
