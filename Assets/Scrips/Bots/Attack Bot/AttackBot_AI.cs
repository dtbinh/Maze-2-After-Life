using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackBot_AI : Enemy_AI {
	public float attackRate;
	public float fieldOfViewAngle;
	bool playerInRange = false;
	float searchInterval = 2f;
	float searchTimer;

	// Update is called once per frame
	void Update () {
		_state = (PlayerHealth.isAlive)?_state:State.Idle;
		searchTimer -= Time.deltaTime;
		timer -= Time.deltaTime;

		switch(_state){
		case State.Init:
			Init ();
			break;
		case State.Attack:
			/*if(nav.remainingDistance < 15f){
				nav.SetDestination(player.transform.position);
			}else if(searchTimer < 0){
				searchTimer = searchInterval;
				nav.SetDestination(player.transform.position);
			}*/
			nav.SetDestination(player.transform.position);

			if(timer <= 0 && playerInRange){
				timer = attackRate;
				AttackPlayer();
			}
			break;
		case State.Idle:
			break;
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject == player){
			playerInRange = true;
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject == player){
			playerInRange = false;
		}
	}

	void Init(){
		_state = State.Attack;

		nav.SetDestination(player.transform.position);
		nav.speed = speed * GameMaster.enemySpeedUpgrade;

	}

	protected void AttackPlayer(){
		// Create a vector from the enemy to the player and store the angle between it and forward.
		Vector3 direction = player.transform.position - transform.position;
		float angle = Vector3.Angle(direction, transform.forward);
		// If the angle between forward and where the player is, is less than half the angle of view...
		if(angle < fieldOfViewAngle * 0.5f){

			var ray = new Ray(transform.position, direction.normalized);
			RaycastHit hit;
			// ... and if a raycast towards the player hits something...
			if(Physics.Raycast(ray, out hit, attackRange, targetLayer)){
				GameObject go = hit.collider.gameObject;
				// ... and if the raycast hits the player...
				if(go.tag == player.tag){
					// ... the player is in sight.
					go.GetComponent<PlayerHealth>().TakeDamage(damage * GameMaster.enemyPowerUpgrade);
				}
			}
		}
	}
}
