using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMove : MonoBehaviour
{
    public float speed = 4f;
    public Rigidbody2D rb;
    public Vector2 direction;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");      
        Destroy(gameObject,10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                //Debug.Log("player hit");
                //Player_controller.healthPoints--;
                player.GetComponentInParent<Player_controller>().takeDamage();
                Destroy(gameObject);
                break;
        }
    }
}
