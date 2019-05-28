using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{

    public int health;
    public GameObject blood;
    private float dazedTime;
    public float startDazedTime;
    public GroundEnemy skeleton;
    public FlyingEnemy bat;

    public float flashTime;
    Color origionalColor;
    [HideInInspector][SerializeField] new Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        origionalColor = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        //If health reaches 0 the enemy dies
        if (health <= 0)
        {
            Destroy(gameObject);
            Instantiate(blood, transform.position, Quaternion.identity);
        }

        if (dazedTime <= 0)
        { //Add multiplier too
            if (this.tag == "Skeleton")
            {
                skeleton = gameObject.GetComponent<GroundEnemy>();
                skeleton.moveSpeed = 1;
            }
            else if (this.tag == "Bat")
            {
                bat = gameObject.GetComponent<FlyingEnemy>();
                bat.moveSpeed = 2;
            }
        }
        else
        {
            if (this.tag == "Skeleton")
            {
                skeleton = gameObject.GetComponent<GroundEnemy>();
                skeleton.moveSpeed = 0;
            }
            else if (this.tag == "Bat")
            {
                bat = gameObject.GetComponent<FlyingEnemy>();
                bat.moveSpeed = 0;
            }
            dazedTime -= Time.deltaTime;
        }
    }

    //Enemy receives damage when the player attacks
    public void takeDamage(int damage)
    {
        dazedTime = startDazedTime;
        FlashRed();
        health = health - damage;
    }

    void FlashRed()
    {
        renderer.material.color = Color.red;
        Invoke("ResetColor", flashTime);
    }
    void ResetColor()
    {
        renderer.material.color = origionalColor;
    }


}