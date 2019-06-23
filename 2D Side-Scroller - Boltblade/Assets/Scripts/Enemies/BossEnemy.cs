using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour{
    public float dirX;
    public float moveSpeed = 1f;
    public float timeToTeleport = 3f;
    public float timeToAction = 3f;
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
    public float beamRadius;
    public float bulletMoveSpeed;
    public Vector2 teleportSpot1;
    public Vector2 teleportSpot2;
    public Vector2 teleportSpot3;
    public bool shotRotated;
    public string currentAction;
    public int teleportPosition;

    void Start(){
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        radius = 5f;
        beamRadius = 3f;
        bulletMoveSpeed = 5f;
        teleportSpot1 = spots[0].position;
        teleportSpot2 = spots[1].position;
        teleportSpot3 = spots[2].position;
        transform.position = teleportSpot1;
        teleportPosition = 1;
        currentPosition = transform;
        shotRotated = false;
        currentAction = "Teleport";
    }

    // Update is called once per frame
    void Update(){
        if(timeToAction <= 0){
            ChooseAction();
        }
        else{
            timeToAction -= Time.deltaTime;
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

    void AttackRadius(int numberOfProjectiles, Vector2 startPoint){
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
        timeToAction = UnityEngine.Random.Range( 1.0f, 3.0f );
	}

    void AttackBeam(int numberOfProjectiles, Vector2 startPoint){
        float projectileDirXposition;
		float projectileDirYposition;
        Vector2 projectileVector;
		Vector2 projectileMoveDirection;
		float angleStep = 90f / numberOfProjectiles;
		float angle = 45f;
        int waitingTime = 0;
		for (int i = 0; i < numberOfProjectiles; i++) {
			projectileDirXposition = startPoint.x + Mathf.Sin ((angle * Mathf.PI) / 180) * beamRadius;
			projectileDirYposition = startPoint.y + Mathf.Cos ((angle * Mathf.PI) / 180) * beamRadius;
			projectileVector = new Vector2 (projectileDirXposition, projectileDirYposition);
			projectileMoveDirection = (projectileVector - startPoint).normalized * bulletMoveSpeed;
            StartCoroutine ( shoot(projectileMoveDirection, startPoint, waitingTime) );
			angle += angleStep;
            waitingTime++;
		}
        for (int i = 0; i < numberOfProjectiles; i++) {
			projectileDirXposition = startPoint.x + Mathf.Sin ((angle * Mathf.PI) / 180) * beamRadius;
			projectileDirYposition = startPoint.y + Mathf.Cos ((angle * Mathf.PI) / 180) * beamRadius;
			projectileVector = new Vector2 (projectileDirXposition, projectileDirYposition);
			projectileMoveDirection = (projectileVector - startPoint).normalized * bulletMoveSpeed;
            StartCoroutine ( shoot(projectileMoveDirection, startPoint, waitingTime) );
			angle -= angleStep;
            waitingTime++;
		}
        timeToAction = UnityEngine.Random.Range( 3.0f, 4.0f );
	}

    public IEnumerator shoot (Vector2 projectileMoveDirection, Vector2 startPoint, float waitingTime) {
        //Wait for some time
        yield return new WaitForSeconds (0.2f * waitingTime);
        //Instantiate your projectile
        var projectile = Instantiate (shot, startPoint, Quaternion.identity);
		projectile.GetComponent<Rigidbody2D> ().velocity = new Vector2 (projectileMoveDirection.x * dirX, projectileMoveDirection.y);
    }

    public void ChooseAction(){
        float prob = UnityEngine.Random.Range( 0f, 1.0f );
        if (currentAction.Equals("Teleport")){
            if(prob < 0.5){
                Teleport();
                currentAction = "Teleport";
            } else if( prob >= 0.5 && prob < 0.8){
                AttackRadius(8, transform.position);
                currentAction = "ShotRadius";
            } else if(prob >= 0.8){
                AttackBeam(9, transform.position);
                currentAction = "ShotWave";
            }
            Debug.Log(currentAction);
        } else if (currentAction.Equals("ShotWave")){
            if(prob < 0.4){
                Teleport();
                currentAction = "Teleport";
            } else if( prob >= 0.4 && prob < 0.75){
                AttackRadius(8, transform.position);
                currentAction = "ShotRadius";
            } else if(prob >= 0.75){
                AttackBeam(9, transform.position);
                currentAction = "ShotWave";
            }
            Debug.Log(currentAction);
        } else if (currentAction.Equals("ShotRadius")){
            if(prob < 0.3){
                Teleport();
                currentAction = "Teleport";
            } else if( prob >= 0.3 && prob < 0.75){
                AttackRadius(8, transform.position);
                currentAction = "ShotRadius";
            } else if(prob >= 0.75){
                AttackBeam(9, transform.position);
                currentAction = "ShotWave";
            }
            Debug.Log(currentAction);
        }
    }

    void moveIdle(Vector2 position){
        Vector2 newPosition;
        newPosition.x = position.x + UnityEngine.Random.Range( 1.0f, 3.0f );
        newPosition.y = position.y;
        flipX = UnityEngine.Random.Range( -1.0f, 1.0f );
        if(flipX < 0){
            newPosition.x *= -1;
        }
        transform.position = Vector2.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }

    public void Teleport(){
        int newPosition;
        if (teleportPosition == 1){
            newPosition = (int) UnityEngine.Random.Range( 0f, 2.0f );
            if(newPosition < 1){
                transform.position = teleportSpot2;
                currentPosition.position = teleportSpot2;
                teleportPosition = 2;
            } else {
                transform.position = teleportSpot3;
                currentPosition.position = teleportSpot3;
                teleportPosition = 3;
            }
        } else if (teleportPosition == 2){
            newPosition = (int) UnityEngine.Random.Range( 0f, 2.0f );
            if(newPosition < 1){
                transform.position = teleportSpot1;
                currentPosition.position = teleportSpot1;
                teleportPosition = 1;
            } else {
                transform.position = teleportSpot3;
                currentPosition.position = teleportSpot3;
                teleportPosition = 3;
            }
        } else if (teleportPosition == 3){
            newPosition = (int) UnityEngine.Random.Range( 0f, 2.0f );
            if(newPosition < 1){
                transform.position = teleportSpot1;
                currentPosition.position = teleportSpot1;
                teleportPosition = 1;
            } else {
                transform.position = teleportSpot2;
                currentPosition.position = teleportSpot2;
                teleportPosition = 2;
            }
        }
        timeToAction = UnityEngine.Random.Range( 1.0f, 2.0f );
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