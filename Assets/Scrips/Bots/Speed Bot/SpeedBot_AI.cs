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
			Patrol();
			break;
		case State.Notified:
			Notified();
			break;
		case State.Danger:
			Warning();
			break;
		case State.Idle:
			break;
		}
	}

	void Init(){
		_state = State.Patrol;
	}

	void Patrol(){
		if (DLSearch (gridMap.GetBlock (transform.position), senseRange))
			_state = State.Notified;

		if(!patrolling){
			patrolling = true;
			// Get neighbours from the current block
			var neighbours = gridMap.GetNeighbours(transform.position);
			// Check if more than 1 neighbours...
			if(neighbours.Count > 1){
				// Remove the last block where came from neighbours
				foreach(Node n in neighbours){
					if(lastNode.worldPosition == n.worldPosition){
						neighbours.Remove(n);
						lastNode = gridMap.GetBlock(transform.position);
						break;
					}
				}
				// Update the new block to patrol
				patrolNode = neighbours[Random.Range(0, neighbours.Count)];
			}else{
				lastNode = gridMap.GetBlock(transform.position);
				patrolNode = neighbours[0];
			}
		}else{
			lookAt(patrolNode.worldPosition);
			// Move to patrol block
			transform.position = Vector3.MoveTowards (transform.position, patrolNode.worldPosition, speed * Time.deltaTime);
			// Stop patrolling when arrives at patrol block
			patrolling &= transform.position != patrolNode.worldPosition;
		}
	}

	void Notified(){
		timer -= Time.deltaTime;
		if(timer < 0){
			timer = searchInterval;
			var nearbyBlocks = NearbyBlocks(gridMap.GetBlock(transform.position), senseRange);
			foreach(Node n in nearbyBlocks){
				var colliders = Physics.OverlapSphere(n.worldPosition, 5f);
				foreach(Collider c in colliders){
					Enemy_AI ai = c.gameObject.GetComponent<Enemy_AI>();
					if(ai != null)
						ai._state = State.Notified;
				}
			}
			if(playerLastLocation != gridMap.GetBlock(player.transform.position)){
				_state = State.Patrol;
				patrolling = false;
			}
		}
	}

	void Warning(){
		skin.material.color = Color.green;
		if (!escaping) {
			var neighbours = gridMap.GetNeighbours (transform.position);
			escapeNode = gridMap.GetBlock (transform.position);
			foreach (Node n in neighbours) {
				escapeNode = Vector3.Distance (escapeNode.worldPosition, player.transform.position) > Vector3.Distance (n.worldPosition, player.transform.position) ? escapeNode : n;
			}
			escaping = true;
		}
		transform.position = Vector3.MoveTowards(transform.position, escapeNode.worldPosition, speed * Time.deltaTime);
		lookAt(escapeNode.worldPosition);

		//escaping = !Equals (transform.position, escapeNode.worldPosition);
		if(Equals (transform.position, escapeNode.worldPosition)){
			var ray = new Ray(transform.position, player.transform.position - transform.position);
			RaycastHit hit;
			// ... and if a raycast towards the player hits something...
			if(Physics.Raycast(ray, out hit, 30f, targetLayer)){
				// ... and if the raycast hits the player...
				if (hit.transform.tag != player.tag) {
					_state = State.Patrol;
					patrolling = false;
				}
			}
			escaping = false;
		}
	}

	bool DLSearch(Node node, int depth){
		int currentDepth = 0;
		var visited = new List<Node>();
		var open = new Stack<Node>();
		open.Push(node);
		while(open.Count > 0){
			var currentNode = open.Pop();
			if(gridMap.CompareBlocks(player.transform.position,currentNode.worldPosition)){
				playerLastLocation = gridMap.GetBlock(player.transform.position);
				return true;
			}

			currentDepth++;
			if(currentDepth == depth)
				currentDepth--;
			else{
				visited.Add(currentNode);
				foreach(Node neighbours in gridMap.GetNeighbours(currentNode.worldPosition)){
					if(!visited.Contains(neighbours))
						open.Push(neighbours);
				}
			}
		}
		return false;
	}

	List<Node> NearbyBlocks(Node node, int depth){
		var visited = new List<Node>();
		int index = 0;
		int loop = 1;
		int total = 0;
		visited.Add(node);
		while(loop > 0){
			int i = 0;
			foreach(Node n in gridMap.GetNeighbours(visited[index].worldPosition)){
				if(!visited.Contains(n)){
					visited.Add(n);
					i++;
				}
			}
			index++;
			loop--;
			total += i;
			if(loop == 0 && depth > 1){
				depth--;
				loop = total;
				total = 0;
			}
		}
		return visited;
	}
}
