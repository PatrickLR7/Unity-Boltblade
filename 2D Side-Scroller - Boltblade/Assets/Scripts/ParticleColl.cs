using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle hit!");
        if (other.layer == 8) {
            other.GetComponent<EnemyDeath>().takeDamage(300);
        }

    }
}
