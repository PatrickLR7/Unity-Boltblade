using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_controller : MonoBehaviour{
    public float speed = 25f;
    public float maxSpeed = 5f;
    private Rigidbody2D playerRB2D;
    public int jumpPower = 12;
    private bool jump;
    public float desiredx;
    private Animator anim;
    public bool grounded;
    public bool facingRight = true;
    public int healthPoints;
    public static bool canTakeDamage = true;
    public static float timeInvincible = 1f;
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public Animator playerAnim;
    public float attackRange;
    public int damage;
    public ParticleSystem especialAttack;
    private float timeBtwEspecial;
    public float startTimeBtwEspecial;
    public float attackDuration;
    public GameObject blood;
    private float dazedTime;
    public float startDazedTime;
    public float flashTime;
    Color origionalColor;
    [HideInInspector] [SerializeField] new Renderer renderer;
    public Text livesText;
    public Image specialAttackImage;
    public ParticleSystem slash;

    public int specialAttackPoints;

    // Start is called before the first frame update
    void Start(){

        playerRB2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAnim = GetComponent<Animator>();
        especialAttack.Stop();
        timeBtwEspecial = 0;
        renderer = GetComponent<SpriteRenderer>();
        origionalColor = renderer.material.color;
        startDazedTime = 0.6f;
        flashTime = 1;
        livesText.text = healthPoints.ToString();
        specialAttackImage.GetComponent<Image>().color = new Color32(255, 255, 225, 225);
        slash.Stop();
        specialAttackPoints = 0;

    }

    // Update is called once per frame
    void Update(){
        anim.SetFloat("speed", Mathf.Abs(playerRB2D.velocity.x));
        anim.SetBool("grounded", grounded);
        //Controls Player Attack
        if (timeBtwAttack <= 0){
            if (Input.GetKey("q") || Input.GetMouseButtonDown(0)){
                playerAnim.SetTrigger("attack2");
                slash.Play();
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++){
                    if (enemiesToDamage[i].gameObject.name.Equals("EnemyCollider")){
                        enemiesToDamage[i].GetComponentInParent<EnemyDeath>().takeDamage(damage);
                        specialAttackPoints++;
                    } else if (enemiesToDamage[i].gameObject.tag.Equals("Shot")){
                        Destroy(enemiesToDamage[i].gameObject);
                        specialAttackPoints++;
                    } else if (enemiesToDamage[i].gameObject.tag.Equals("Boss")){
                        enemiesToDamage[i].GetComponent<EnemyDeath>().takeDamage(UnityEngine.Random.Range(75f, 100f));
                        specialAttackPoints++;
                    }
                    else{
                        Debug.Log(enemiesToDamage[i]);
                        enemiesToDamage[i].GetComponent<EnemyDeath>().takeDamage(damage);
                        specialAttackPoints++;
                    }
                }
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else{
            timeBtwAttack -= Time.deltaTime;
            slash.Stop();
        }
        //Special Attack
        /* if (timeBtwEspecial <= 0){
             specialAttackImage.GetComponent<Image>().color = new Color32(255, 255, 225, 225);
             if (Input.GetKey("e")){
                 attackDuration = 0.5f;
                 especialAttack.Play();
                 timeBtwEspecial = startTimeBtwEspecial;
             }
         }
         else {
             timeBtwEspecial -= Time.deltaTime;
             attackDuration -= Time.deltaTime;
             specialAttackImage.GetComponent<Image>().color = new Color32(255, 255, 225, 100);
         }
         if (attackDuration <= 0) {
             especialAttack.Stop();
         }*/
        if (specialAttackPoints >= 15)
        {
            specialAttackImage.GetComponent<Image>().color = new Color32(255, 255, 225, 225);
            if (Input.GetKey("e"))
            {

                especialAttack.Play();
                specialAttackPoints = 0;
                attackDuration = 0.5f;
            }

        }
        else
        {
            specialAttackImage.GetComponent<Image>().color = new Color32(255, 255, 225, 100);
            attackDuration -= Time.deltaTime;

        }

        if (attackDuration <= 0)
        {
            especialAttack.Stop();
           
        }
        

    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    //Should always be used for physics calculations.
    private void FixedUpdate(){
        //Controls Player Movement
        float h = Input.GetAxis("Horizontal");
        playerRB2D.velocity = new Vector2(maxSpeed * h, playerRB2D.velocity.y);
        if (Input.GetButton("Jump") && grounded == true){
            playerRB2D.velocity = new Vector2(0, 8);
        }
        float limitedSpeed = Mathf.Clamp(playerRB2D.velocity.x, -maxSpeed, maxSpeed);
        playerRB2D.velocity = new Vector2(limitedSpeed, playerRB2D.velocity.y);
        if (Input.GetKey("right") || Input.GetKey("d")) {
            if (h > 0.1f && !facingRight) {
                Flip(); //Rotate Right
            }
        }
        if (Input.GetKey("left") || Input.GetKey("a")) {
            if (h < 0.1f && facingRight) {
                Flip(); //Rotate Left
            }
        }
        if (healthPoints == 0){
            Instantiate(blood, transform.position, Quaternion.identity);
        }

        if (!canTakeDamage)
        {
            timeInvincible -= Time.deltaTime;
        }
        if(timeInvincible <= 0)
        {
            canTakeDamage = true;
            if (healthPoints <= 0){
                SceneManager.LoadScene(3);
            }
        }
    }

    void Flip(){
        facingRight = !facingRight;
        transform.Rotate(Vector3.up * 180);
    }

    public void takeDamage(){
        if(canTakeDamage){
            Debug.Log("HP left:" + --healthPoints);
            canTakeDamage = false;
            timeInvincible = 1f;
            dazedTime = startDazedTime;
            FlashRed();
            livesText.text = healthPoints.ToString();
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
