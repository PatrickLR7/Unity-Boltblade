using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDeath : MonoBehaviour{

    public float health;
    public GameObject blood;
    private float dazedTime;
    public float startDazedTime;
    public GroundEnemy skeleton;
    public FlyingEnemy bat;
    public BossEnemy boss;
    public float flashTime;
    Color origionalColor;
    [HideInInspector][SerializeField] new Renderer renderer;
    public bool bossCanTakeDamage = true;
    public float bossTimeInvincible = 1f;
    public float bossHealth;

    // Start is called before the first frame update
    void Start(){
        renderer = GetComponent<SpriteRenderer>();
        origionalColor = renderer.material.color;
        if (this.tag == "Boss"){
            bossHealth = health;
        }
    }

    // Update is called once per frame
    void Update(){
        //If health reaches 0 the enemy dies
        if (health <= 0){
            Destroy(gameObject);
            Instantiate(blood, transform.position, Quaternion.identity);
            if (this.tag == "Boss"){
                SceneManager.LoadScene(5);
            }
        }
        if (dazedTime <= 0){ //Add multiplier too
            if (this.tag == "Skeleton"){
                skeleton = gameObject.GetComponent<GroundEnemy>();
                skeleton.moveSpeed = 1;
            }
            else if (this.tag == "Bat"){
                bat = gameObject.GetComponent<FlyingEnemy>();
                bat.moveSpeed = 2;
            }
            else if (this.tag == "Boss"){
                boss = gameObject.GetComponent<BossEnemy>();
                boss.moveSpeed = 1;
            }
        }
        else{
            if (this.tag == "Skeleton"){
                skeleton = gameObject.GetComponent<GroundEnemy>();
                skeleton.moveSpeed = 0;
            }
            else if (this.tag == "Bat"){
                bat = gameObject.GetComponent<FlyingEnemy>();
                bat.moveSpeed = 0;
            }
            else if (this.tag == "Boss"){
                boss = gameObject.GetComponent<BossEnemy>();
                boss.moveSpeed = 0;
            }
            dazedTime -= Time.deltaTime;
        }
    }

    void FixedUpdate()
        {
            if (!bossCanTakeDamage)
            {
                bossTimeInvincible -= Time.deltaTime;
            }
            if(bossTimeInvincible <= 0)
            {
                bossCanTakeDamage = true;
            }
        }

    //Enemy receives damage when the player attacks
    public void takeDamage(float damage){
        dazedTime = startDazedTime;
        FlashRed();
        if (this.tag == "Boss")
        {
            if(bossCanTakeDamage)
            {
                Debug.Log("Boss took damage");
                health = health - damage;
                gameObject.GetComponent<GameHandler>().ReduceHealthBar(health/bossHealth);
                Debug.Log("Boss health left: " + health);
                bossCanTakeDamage = false;
                bossTimeInvincible = 1f;
            }
        }
        else
        {
            health = health - damage;
        }
    }

    void FlashRed(){
        renderer.material.color = Color.red;
        Invoke("ResetColor", flashTime);
    }
    
    void ResetColor(){
        renderer.material.color = origionalColor;
    }


}