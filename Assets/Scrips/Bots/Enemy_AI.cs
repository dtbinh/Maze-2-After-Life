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

	public State _state;


	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
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
}
