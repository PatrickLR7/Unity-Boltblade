using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour{
    public float dirX;
    public float moveSpeed = 1f;
    public float timeToTeleport = 3f;
    public float timeToShoot = 1.5f;
    public Rigidbody2D rb;
    public Vector3 localScale;
    public Transform target;
    public bool facingRight = false;
    public GameObject player;
    public float flipX;
    public float flipY;
    public GameObject shot;
    public Transform[] spots;
    public Transform currentPosition;
    public float radius;
    public float bulletMoveSpeed;
    public Vector2 teleportSpot1;
    public Vector2 teleportSpot2;
    public bool teleported;
    public bool shotRotated;

    void Start(){
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        radius = 5f;
        bulletMoveSpeed = 5f;
        teleported = false;
        teleportSpot1 = spots[1].position;
        teleportSpot2 = spots[0].position;
        transform.position = teleportSpot1;
        currentPosition = transform;
        shotRotated = false;
    }

    // Update is called once per frame
    void Update(){
        //Choose between 2 points to teleport
        if(timeToTeleport <= 0){   //teleport
            Teleport();
            timeToTeleport = 3f;
        }
        else{
            timeToTeleport -= Time.deltaTime;
        }
        if(timeToShoot <= 0){   //Shoot
            SpawnProjectiles(8, transform.position);
            timeToShoot = 2f;
        }else{
            timeToShoot -= Time.deltaTime;
        }
        if (transform.position.x < target.position.x - 0.5){
            dirX = 1f;
        }else if (transform.position.x > target.position.x + 0.5){
            dirX = -1f;
        }
    }

    void FixedUpdate(){
        moveIdle(currentPosition.position);
    }

    void SpawnProjectiles(int numberOfProjectiles, Vector2 startPoint){
		float angleStep = 360f / numberOfProjectiles;
		float angle;
        if (!shotRotated){
            angle = 0f;
            shotRotated = true;
        } else {
            angle = angleStep/2;
            shotRotated = false;
        }
		for (int i = 0; i < numberOfProjectiles; i++) {
			float projectileDirXposition = startPoint.x + Mathf.Sin ((angle * Mathf.PI) / 180) * radius;
			float projectileDirYposition = startPoint.y + Mathf.Cos ((angle * Mathf.PI) / 180) * radius;
			Vector2 projectileVector = new Vector2 (projectileDirXposition, projectileDirYposition);
			Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * bulletMoveSpeed;
			var proj = Instantiate (shot, startPoint, Quaternion.identity);
			proj.GetComponent<Rigidbody2D> ().velocity = new Vector2 (projectileMoveDirection.x, projectileMoveDirection.y);
			angle += angleStep;
		}
	}

    void moveIdle(Vector2 position){
        Vector2 newPosition;
        newPosition.x = position.x + UnityEngine.Random.Range( 3.0f, 5.0f );
        newPosition.y = position.y + UnityEngine.Random.Range( 3.0f, 5.0f );
        flipX = UnityEngine.Random.Range( -1.0f, 1.0f );
        flipY = UnityEngine.Random.Range( -1.0f, 1.0f );
        if(flipX < 0){
            newPosition.x *= -1;
        }
        if(flipY < 0){
            newPosition.y *= -1;
        }
        transform.position = Vector2.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }

    public void Teleport(){
        if(teleported){
            transform.position = teleportSpot1;
            currentPosition.position = teleportSpot1;
            teleported = false;
        } else {
            transform.position = teleportSpot2;
            currentPosition.position = teleportSpot2;
            teleported = true;
        }
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
                player.GetComponentInParent<Player_controller>().takeDamage();
                rb.velocity = Vector2.zero;
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collision){
        switch (collision.gameObject.tag){
            case "Player":
                player.GetComponentInParent<Player_controller>().takeDamage();
                rb.velocity = Vector2.zero;
                break;
        }
    }
}