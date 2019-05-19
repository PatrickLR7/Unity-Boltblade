﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterEnemy : MonoBehaviour
{
    public Transform target;
    public float dirX;
    float teleportTimer;
    float flipX;
    float flipY;
    public Vector3 localScale;
    public bool facingRight = false;

    public GameObject laser;

    public float fireRate;
    public float nextFire;
    // Start is called before the first frame update
    void Start()
    {
        dirX = -1f;
        teleportTimer = 5f;
        fireRate = 2f;
        nextFire = Time.time;
        localScale = transform.localScale;
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
        checkIfTimeToFire();
        if (teleportTimer <= 0)
        {
            Teleport();
        } 
        else
        {
            teleportTimer -= Time.deltaTime;
        }
    }

    public void checkIfTimeToFire(){
        if(Time.time > nextFire){
            
            Instantiate(laser, transform.position, Quaternion.identity);

            nextFire = Time.time + fireRate;
        }
    }

    void Teleport()
    {
        Vector3 position = target.transform.position;
        position.x = position.x + UnityEngine.Random.Range( 2.0f, 3.0f );
        position.y = position.y + UnityEngine.Random.Range( 2.0f, 3.0f );
        flipX = UnityEngine.Random.Range( -1.0f, 1.0f );
        flipY = UnityEngine.Random.Range( -1.0f, 1.0f );
        if(flipX < 0){
            position.x *= -1;
        }
        if(flipY < 0){
            position.y *= -1;
        }
        transform.position = position;
        teleportTimer = 5f;
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
