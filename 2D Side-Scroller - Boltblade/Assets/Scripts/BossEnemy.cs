using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public float dirX;
    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    public Vector3 localScale;
    public Transform target;
    public bool facingRight = false;
    public GameObject player;
    public float flipX;
    public float flipY;

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Choose between 4 points to teleport
        */
        if (transform.position.x < target.position.x - 0.5)
        {
            dirX = 1f;
        }
        else if (transform.position.x > target.position.x + 0.5)
        {
            dirX = -1f;
        }
        
        //moveIdle();
    }

    void moveIdle(Vector2 position)
    {
        Vector2 newPosition;
        newPosition.x = position.x + UnityEngine.Random.Range( 2.0f, 3.0f );
        newPosition.y = position.y + UnityEngine.Random.Range( 2.0f, 3.0f );
        flipX = UnityEngine.Random.Range( -1.0f, 1.0f );
        flipY = UnityEngine.Random.Range( -1.0f, 1.0f );
        if(flipX < 0){
            newPosition.x *= -1;
        }
        if(flipY < 0){
            newPosition.y *= -1;
        }
        transform.position = Vector2.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                //Debug.Log("player hit");
                //Player_controller.healthPoints--;
                player.GetComponentInParent<Player_controller>().takeDamage();
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                //Debug.Log("player hit");
                //Player_controller.healthPoints--;
                player.GetComponentInParent<Player_controller>().takeDamage();
                break;
        }
    }
}