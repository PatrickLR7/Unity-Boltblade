using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour{
    public float speed = 4f;
    public Rigidbody2D rb;
    public Transform target;
    public Vector2 direction;
    public GameObject player;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        direction = (target.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(direction.x, direction.y);
        player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject,5f);
    }

    void OnTriggerEnter2D(Collider2D collision){
        switch (collision.tag){
            case "Player":
                player.GetComponentInParent<Player_controller>().takeDamage();
                Destroy(this.gameObject);
                break;
        }
    }
}
