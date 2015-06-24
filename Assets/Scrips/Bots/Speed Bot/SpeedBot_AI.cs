using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedBot_AI : Enemy_AI {
	public int senseRange;
	public int searchInterval = 3;
	Node playerLastLocation;
	Node escapeNode;
	Quaternion angle;
	bool escaping;

	// Update is called once per frame
	void Update () {
		var ray = new Ray(transform.position, player.transform.position - transform.position);
		RaycastHit hit;
		// ... and if a raycast towards the player hits something...
		if(Physics.Raycast(ray, out hit, 30f, targetLayer)){
			// ... and if the raycast hits the player...
			if (hit.transform.tag == player.tag)
				_state = State.Danger;
		}

		_state = (PlayerHealth.isAlive)?_state:State.Idle;

		switch(_state){
		case State.Init:
			Init ();
			break;
		case State.Patrol:
			break;
		case State.Approach:
			break;
		case State.Danger:
			break;
		case State.Idle:
			break;
		}
	}

	void Init(){
		_state = State.Patrol;
	}
}
