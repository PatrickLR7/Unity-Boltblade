using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public float dirX;
    public float moveSpeed = 1f;
    public float timeToTeleport = 3f;
    public float timeToShoot = 2f;
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

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        radius = 5f;
        bulletMoveSpeed = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Choose between 4 points to teleport
        */
        if(timeToTeleport <= 0)
        {   //teleport
            transform.position = spots[Random.Range(0, spots.Length)].position;
            currentPosition = transform;
            timeToTeleport = 3f;
        }
        else
        {
            timeToTeleport -= Time.deltaTime;
        }

        if(timeToShoot <= 0)
        {   //Shoot
            SpawnProjectiles(8, transform.position);
            timeToShoot = 2f;
        }
        else
        {
            timeToShoot -= Time.deltaTime;
        }

        if (transform.position.x < target.position.x - 0.5)
        {
            dirX = 1f;
        }
        else if (transform.position.x > target.position.x + 0.5)
        {
            dirX = -1f;
        }
    }

    void FixedUpdate(){
        moveIdle(currentPosition.position);
    }

    void SpawnProjectiles(int numberOfProjectiles, Vector2 startPoint)
	{
		float angleStep = 360f / numberOfProjectiles;
		float angle = 0f;

		for (int i = 0; i <= numberOfProjectiles - 1; i++) {
			
			float projectileDirXposition = startPoint.x + Mathf.Sin ((angle * Mathf.PI) / 180) * radius;
			float projectileDirYposition = startPoint.y + Mathf.Cos ((angle * Mathf.PI) / 180) * radius;

			Vector2 projectileVector = new Vector2 (projectileDirXposition, projectileDirYposition);
			Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * bulletMoveSpeed;

			var proj = Instantiate (shot, startPoint, Quaternion.identity);
			proj.GetComponent<Rigidbody2D> ().velocity = 
				new Vector2 (projectileMoveDirection.x, projectileMoveDirection.y);

			angle += angleStep;
		}
	}

    void moveIdle(Vector2 position)
    {
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

    private void LateUpdate()
    {
        CheckPosition();
    }

    void CheckPosition()
    {
        if (dirX > 0)
        {
            facingRight = true;
        }
        else if (dirX < 0)
        {
            facingRight = false;
        }
        if ((facingRight && localScale.x < 0) || (!facingRight && localScale.x > 0))
        {
            localScale.x *= -1;
        }
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                //Debug.Log("player hit");
                //Player_controller.healthPoints--;
                player.GetComponentInParent<Player_controller>().takeDamage();
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                //Debug.Log("player hit");
                //Player_controller.healthPoints--;
                player.GetComponentInParent<Player_controller>().takeDamage();
                break;
        }
    }
}