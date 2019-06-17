using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour {
    float health;
    [SerializeField] HealthBar healthBar;

	void Start () {
        health = 1f;
        healthBar.SetColor(Color.red);
	}

    public void ReduceHealthBar(float currentHealt){
        while(health > 0 && health > currentHealt){
            health -= 0.01f;
            healthBar.SetSize(health);
        }
    }
}
