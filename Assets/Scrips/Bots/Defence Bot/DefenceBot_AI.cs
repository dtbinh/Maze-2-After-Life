using UnityEngine;

public class DefenceBot_AI : Enemy_AI{
	public float chargeTime;
	public float chargeSpeed;
	public float impactForce;
	public AudioSource chargeSound;

	float searchInterval = 2f;
	float t;

	bool targetMarked;
	bool attacked;
	Rigidbody rigidBody;

	// Update is called once per frame
	void Update () {
		_state = (PlayerHealth.isAlive)?_state:State.Idle;
		distanceBetweenPlayer = Vector3.Distance(player.transform.position, transform.position);
		timer -= Time.deltaTime;

		switch(_state){
		case State.Init:
			Init ();
			break;
		case State.Attack:
			Approach();
			break;
		case State.Idle:
			break;
		}
	}

	void Init(){
		rigidBody = GetComponent<Rigidbody>();
		_state = State.Attack;
	}

	void OnCollisionEnter(Collision c){
		if(c.collider.tag == "Player" && targetMarked){
			if ( !attacked){
				attacked = true;
				c.collider.GetComponent<PlayerHealth>().TakeDamage (damage * GameMaster.enemyPowerUpgrade);
				c.collider.GetComponent<Rigidbody>().AddForce(transform.position - c.collider.transform.position * 100);
			}
		}
	}

	void Approach(){
		if(!targetMarked){
			if(nav.remainingDistance < 15f){
				nav.SetDestination(player.transform.position);
			}else if(t < 0){
				t = searchInterval;
				nav.SetDestination(player.transform.position);
			}
			TargetMark();
		}
	}

	void TargetMark(){
		RaycastHit hit;
		if(Physics.SphereCast(transform.position, 0.75f, transform.forward, out hit, attackRange, targetLayer)){
			if(hit.transform.tag == "Player"){
				nav.Stop();
				rigidBody.freezeRotation = true;
				attacked = false;
				targetMarked = true;
				lookAt(hit.point);
				Invoke("Charge", chargeTime);
			}
		}
	}

	void Charge(){
		chargeSound.Play ();
		rigidBody.AddForce(transform.forward*10000);
		Invoke("ApproachAgain", 0.5f);
	}

	void ApproachAgain(){
		nav.Resume();
		targetMarked = false;
		rigidBody.freezeRotation = false;
	}
}
