using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    public float speed = 25f;
    public float maxSpeed = 5f;
    private Rigidbody2D playerRB2D;
    public float jumpPower = 1.5f;
    private bool jump;
    public float desiredx;
    private Animator anim;
    public bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        playerRB2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", Mathf.Abs(playerRB2D.velocity.x));
        anim.SetBool("grounded", grounded);
    }

    //Should always be used for physics calculations.
    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        playerRB2D.AddForce(Vector2.right * speed * h);

        if (Input.GetButton("Jump"))
        {
            playerRB2D.velocity = new Vector2(0, 5);
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

    }
}
