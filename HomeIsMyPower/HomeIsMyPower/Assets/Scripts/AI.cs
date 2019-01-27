using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
    public GameObject playerObj;
    private Vector3 playerPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        playerPos = playerObj.transform.position;
	}

    void FindPlayerInRadius() {

    }
}
