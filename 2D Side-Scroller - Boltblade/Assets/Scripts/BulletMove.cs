using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public Transform target;
    public Vector2 direction;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        direction = (target.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(direction.x, direction.y);
        Destroy(gameObject,3f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                Debug.Log("player hit");
                Destroy(gameObject);
                break;
        }
    }
}
