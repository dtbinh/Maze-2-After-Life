using UnityEngine;

public enum State{
	Idle,
	Init,
	Patrol,
	Attack,
}

public class Enemy_AI : MonoBehaviour {
	public float attackRange;
	public int damage;
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
		transform.GetChild (0).transform.GetChild (0).GetComponent<Renderer>().material.color = 
			new Color(GameMaster.enemyColour, GameMaster.enemyColour, GameMaster.enemyColour, 1);
	}

	/* Facing the player
	 */
	protected void lookAt(Vector3 pos){
		Quaternion angle;
		angle = pos != transform.position ? Quaternion.LookRotation (pos - transform.position) : Quaternion.identity;
		transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * damping);
	}
}
