﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour{
    public float dirX;
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public Vector3 localScale;
    public Transform target;
    public bool facingRight = false;
    public GameObject player;

    // Start is called before the first frame update
    void Start(){
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (Wave_Spawner.nextWave > 0){
            float multiplier = ((Wave_Spawner.nextWave * 5.0f) / 100.0f) + 1.0f;
            //Debug.Log("Multiplier is: " + multiplier.ToString("n2"));
            moveSpeed = moveSpeed * multiplier;
            Debug.Log("Movement speed is: " + moveSpeed);
        }
    }

    // Update is called once per frame
    void Update(){
        if (transform.position.x < target.position.x - 0.5){
            dirX = 1f;
        }
        else if (transform.position.x > target.position.x + 0.5){
            dirX = -1f;
        }
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    private void LateUpdate(){
        CheckPosition();
    }

    void CheckPosition(){
        if (dirX > 0){
            facingRight = true;
        }
        else if (dirX < 0){
            facingRight = false;
        }
        if ((facingRight && localScale.x < 0) || (!facingRight && localScale.x > 0)){
            localScale.x *= -1;
        }
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        switch (collision.gameObject.tag){
            case "Player":
                //Debug.Log("player hit");
                //Player_controller.healthPoints--;
                player.GetComponentInParent<Player_controller>().takeDamage();
                rb.velocity = Vector2.zero;
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collision){
        switch (collision.gameObject.tag){
            case "Player":
                //Debug.Log("player hit");
                //Player_controller.healthPoints--;
                player.GetComponentInParent<Player_controller>().takeDamage();
                rb.velocity = Vector2.zero;
                break;
        }
    }
}
