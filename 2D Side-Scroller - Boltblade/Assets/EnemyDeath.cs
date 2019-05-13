using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{

    public int health;
    public GameObject blood;
    private float dazedTime;
    public float startDazedTime;
    public EnemyJumpOverObstacles skeleton;
    public EnemyFollow bat;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //If health reaches 0 the enemy dies
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        //Enemy stops moving when hit
        if (dazedTime <= 0)
        {
            if (this.tag == "Skeleton")
            {
                skeleton = gameObject.GetComponent<EnemyJumpOverObstacles>();
                skeleton.moveSpeed = 5;
            }
            else if (this.tag == "Bat") {
                bat = gameObject.GetComponent<EnemyFollow>();
                bat.moveSpeed = 5;
            }
           
        }
        else
        {
            if (this.tag == "Skeleton")
            {
                skeleton = gameObject.GetComponent<EnemyJumpOverObstacles>();
                skeleton.moveSpeed = 0;
            }
            else if (this.tag == "Bat")
            {
                bat = gameObject.GetComponent<EnemyFollow>();
                bat.moveSpeed = 0;
            }
            dazedTime -= Time.deltaTime;
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
    */

    //Enemy receives damage when the player attacks
    public void takeDamage(int damage)
    {
        dazedTime = startDazedTime;
        Instantiate(blood, transform.position, Quaternion.identity);
        health = health - damage;
    }
}