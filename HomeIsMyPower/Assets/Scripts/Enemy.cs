using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public NavMeshAgent agent;
    public GameObject Player;

	// Use this for initialization
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
		if(Vector3.Distance(transform.position, Player.transform.position) < 20) {
			agent.destination = Player.transform.position;
		}
	}
}
