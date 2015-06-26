using UnityEngine;

public enum State{
	Idle,
	Init,
	Patrol,
	Attack,
}

public class Enemy_AI : MonoBehaviour {
	public float speed;
	public float damping;
	public LayerMask targetLayer;
	public LayerMask obstacleLayer;
	protected float timer;
	protected float distanceBetweenPlayer;
	protected GameObject player;
	protected NavMeshAgent nav;

	protected Renderer skin;

	public State _state;


	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
		skin = transform.GetChild (0).transform.GetChild (0).GetComponent<Renderer> ();
		nav = GetComponent<NavMeshAgent>();
		nav.speed = speed + GameMaster.enemySpeedUpgrade;
	}

	/* Facing the player
	 */
	protected void lookAt(Vector3 pos){
		Quaternion angle;
		angle = pos != transform.position ? Quaternion.LookRotation (pos - transform.position) : Quaternion.identity;
		transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * damping);
	}

	void SpawnDirection() {
		float max = 0;
		var hits = new RaycastHit[4];
		Physics.Raycast(transform.position, transform.forward, out hits[0], obstacleLayer);
		Physics.Raycast(transform.position, -transform.forward, out hits[1], obstacleLayer);
		Physics.Raycast(transform.position, transform.right, out hits[2], obstacleLayer);
		Physics.Raycast(transform.position, -transform.right, out hits[3], obstacleLayer);
		
		foreach(RaycastHit hit in hits)
			max = Mathf.Max(max, hit.distance);
		
		if(max == hits[1].distance)
			transform.Rotate(0, 180, 0);
		else if(max == hits[2].distance)
			transform.Rotate(0, 90, 0);
		else if(max == hits[3].distance)
			transform.Rotate(0, 270, 0);
	}
}
