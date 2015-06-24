using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackBot_AI : Aggressive_AI {
	public float attackRate;
	public float fieldOfViewAngle;

	// Update is called once per frame
	void Update () {
		_state = (PlayerHealth.isAlive)?_state:State.Idle;
		distanceBetweenPlayer = Vector3.Distance(player.transform.position, transform.position);

		switch(_state){
		case State.Init:
			Init ();
			break;
		case State.Approach:
			Approach();
			break;
		case State.Idle:
			break;
		}
	}

	void Init(){
		_state = State.Approach;
	}

	void Approach(){
		skin.material.color = Color.yellow;
		timer -= Time.deltaTime;

		if(distanceBetweenPlayer < attackRange){
			lookAt(player.transform.position);
			if(timer <= 0){
				timer = attackRate;
				scanForPlayer();
			}
		}else
			nav.SetDestination(player.transform.position);
	}

	/* Attack the player
	 */
	void Attack(){	
		RaycastHit hit;
		if(Physics.Raycast(transform.position, transform.forward, out hit, attackRange, targetLayer)){
			GameObject go = hit.collider.gameObject;
			if(go.tag == "Player"){
				go.GetComponent<PlayerHealth>().TakeDamage(damage + GameMaster.enemyPowerUpgrade);
			}
		}
	}

	protected void scanForPlayer(){
		// Create a vector from the enemy to the player and store the angle between it and forward.
		Vector3 direction = player.transform.position - transform.position;
		float angle = Vector3.Angle(direction, transform.forward);
		// If the angle between forward and where the player is, is less than half the angle of view...
		if(angle < fieldOfViewAngle * 0.5f)
			CastRay(attackRange, direction);
	}
	
	
	/*
	 * Cast ray to a direction
	 */
	void CastRay(float range, Vector3 direction){
		var ray = new Ray(transform.position, direction.normalized);
		RaycastHit hit;
		// ... and if a raycast towards the player hits something...
		if(Physics.Raycast(ray, out hit, range, targetLayer)){
			GameObject go = hit.collider.gameObject;
			// ... and if the raycast hits the player...
			if(go.tag == player.tag){
				// ... the player is in sight.
				go.GetComponent<PlayerHealth>().TakeDamage(damage + GameMaster.enemyPowerUpgrade);
			}
		}
	}


}
