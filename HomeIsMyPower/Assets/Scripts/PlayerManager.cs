using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    public float health;
    public float healthPercent;
    private int energy;

	// Use this for initialization
	void Start () {
        health = 100.0f;
        energy = 50;
	}
	
	// Update is called once per frame
	void Update () {
        healthPercent = health / 100.0f;
        GameObject.FindGameObjectWithTag("Health").GetComponent<Image>().fillAmount = healthPercent;
        if (Input.GetKeyDown(KeyCode.G)) {
            TakeDamage();
        }
    }

    void TakeDamage(){
        if (health > 0.0f) {
            health -= 5.0f;
        }
    }
}
