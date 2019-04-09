using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    public float speed = 25;
    public float maxSpeed = 5f;
    private Rigidbody2D playerRB2D;
    public float jumpPower = 6.5f;
    private bool jump;

    // Start is called before the first frame update
    void Start()
    {
        playerRB2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //Trabajando con fisicas
    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        playerRB2D.AddForce(Vector2.right * speed * h);

        if (playerRB2D.velocity.x > maxSpeed) {
            playerRB2D.velocity = new Vector2(maxSpeed, playerRB2D.velocity.y);
        }

        if (playerRB2D.velocity.x < -maxSpeed)
        {
            playerRB2D.velocity = new Vector2(-maxSpeed, playerRB2D.velocity.y);
        }
    }
}
