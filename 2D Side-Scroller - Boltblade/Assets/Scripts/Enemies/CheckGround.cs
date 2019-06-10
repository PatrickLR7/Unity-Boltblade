using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour{
    private Player_controller player;
    // Start is called before the first frame update
    void Start(){
        player = GetComponentInParent<Player_controller>();
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnCollisionStay2D(Collision2D collision){
        if (collision.gameObject.tag == "Environment") {
            player.grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.tag == "Environment"){
            player.grounded = false;
        }
    }
}
