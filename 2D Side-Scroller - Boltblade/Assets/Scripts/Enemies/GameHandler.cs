using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {
    float health;
    [SerializeField] HealthBar healthBar;

	void Start () {
        health = 1f;
        healthBar.SetColor(Color.red);
	}

    public IEnumerator ReduceHealthBar(float currentHealt){
        while(health > 0 && health > currentHealt){
            yield return new WaitForSeconds (0.01f);
            health -= 0.001f;
            healthBar.SetSize(health);
        }
    }

}
