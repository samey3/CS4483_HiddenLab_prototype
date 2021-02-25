using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_controller : MonoBehaviour {

	public Transform player;
	public float playerDistance;
	public float awareAI;
	public float minMoveSpeed;
	public float chaseSpeed;
	public float damping = 6.0f;

	public Transform[] navPoints;
	public UnityEngine.AI.NavMeshAgent agent;

	Vector3 lookVec = Vector3.forward;

	void Start () {
		UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.destination = navPoints[Random.Range(0, navPoints.Length)].position; 
		agent.autoBraking = false;
		agent.speed = minMoveSpeed;
	}

	void Update () {
		// Find how far away the player is
		playerDistance = Vector3.Distance (player.position, transform.position);

		// If within chase distance and not hidden
		if (playerDistance < awareAI && !player.GetComponent<Player_controls>().isPlayerHidden()){
			// Look at the player
			lookVec = Vector3.ProjectOnPlane ((player.position - transform.localPosition), Vector3.up);
			transform.rotation = Quaternion.LookRotation(lookVec, Vector3.up);

			// Set the player as the target destination and adjust speed
			agent.destination = player.position;	
			agent.speed = chaseSpeed;
		}
		// No else case, it will track to the last position (e.g. searching for player...)
			
		// Once close enough to a navigation point, choose a new one
		if (agent.remainingDistance < 0.5f){
			agent.destination = navPoints[Random.Range(0, navPoints.Length)].position;
			agent.speed = minMoveSpeed;
		}
	}
}