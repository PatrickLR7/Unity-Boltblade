using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;
    public Rigidbody2D playerRB2D;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitPlayer()
    {
        health--;
        Debug.Log("Player's health = " + health);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Shot"))
        {
            HitPlayer();
            Destroy(collision.gameObject);
        }
    }
}
