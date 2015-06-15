using UnityEngine;

public enum State{
	Idle,
	Init,
	Patrol,
	Notified,
	Danger,
}

public class Enemy_AI : MonoBehaviour {
	public float speed;
	public float damping;
	public bool notified;
	public bool playerInSight;
	public bool patrolling;
	public LayerMask targetLayer;
	public LayerMask obstacleLayer;
	public LayerMask botLayer;
	protected float timer;
	protected float distanceBetweenPlayer;
	protected GameObject player;
	protected GridGenerator gridMap;
	protected NavMeshAgent nav;

	protected Node patrolNode;
	protected Node lastNode;
	protected Renderer skin;

	public State _state;


	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
		gridMap = GameObject.FindGameObjectWithTag("BlockGrid").GetComponent<GridGenerator>();
		lastNode = gridMap.GetBlock(transform.position);
		skin = transform.GetChild (0).transform.GetChild (0).GetComponent<Renderer> ();
		nav = GetComponent<NavMeshAgent>();

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

	/* Facing the player
	 */
	protected void lookAt(Vector3 pos){
		Quaternion angle;
		angle = pos != transform.position ? Quaternion.LookRotation (pos - transform.position) : Quaternion.identity;
		transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * damping);
	}



}
