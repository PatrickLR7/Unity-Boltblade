using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColl : MonoBehaviour{
    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnParticleCollision(GameObject other){
        if (other.layer == 8) {
            if(other.tag.Equals("Boss")){
                other.GetComponent<EnemyDeath>().takeDamage(UnityEngine.Random.Range(100f, 150f));
            } else if (other.tag.Equals("Shot")){
                Destroy(other.gameObject);
            } else {
                other.GetComponent<EnemyDeath>().takeDamage(200);
            }
        }
    }
}
