using UnityEngine;

public class DefenceBot_AI : Aggressive_AI{

	public float OrbRotationSpeed;
	public float chargeTime;
	public float chargeSpeed;
	public float warningTime;
	public float impactForce;
	public AudioSource chargeSound;

	Transform[] orbs;
	bool targetMarked;
	bool attacked;
	Vector3 targetPosition;
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
		case State.Approach:
			Approach();
			break;
		case State.Danger:
			Danger();
			break;
		case State.Idle:
			break;
		}
		RotateOrbs();
	}

	void Init(){
		orbs = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Transform>();
		rigidBody = GetComponent<Rigidbody>();
		_state = State.Approach;
	}

	void OnCollisionEnter(Collision c){
		if(c.collider.tag == "Player" && targetMarked){
			ImpactReceiver forceReceiver = c.collider.GetComponent<ImpactReceiver>();
			if (forceReceiver && !attacked){
				attacked = true;
				c.collider.GetComponent<PlayerHealth>().TakeDamage (damage + GameMaster.enemyPowerUpgrade);
				forceReceiver.AddImpact (transform.forward, impactForce);

				_state = State.Danger;
				timer = warningTime;
			}
		}
	}

	void Approach(){
		if(!targetMarked){
			if(distanceBetweenPlayer < attackRange)
				lookAt(player.transform.position);
			else
				nav.SetDestination(player.transform.position);
			skin.material.color = Color.yellow;
			TargetMark();
		}else if(timer < 0){
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, chargeSpeed * Time.deltaTime);
			if (!chargeSound.isPlaying)
				chargeSound.Play ();
			if(transform.position == targetPosition){
				_state = State.Danger;
				timer = warningTime;
			}
		}
	}
	void Danger(){
		targetMarked = false;
		rigidBody.freezeRotation = false;
		skin.material.color = Color.green;
		if(timer < 0){
			_state = State.Approach;
			attacked = false;
		}
	}

	void RotateOrbs(){
		if(_state != State.Init)
			foreach(Transform t in orbs){
				t.RotateAround(transform.position, new Vector3(0, 1, 0), OrbRotationSpeed * Time.deltaTime);
			}
	}

	void TargetMark(){
		RaycastHit hit;
		if(Physics.Raycast(transform.position, transform.forward, out hit, attackRange, targetLayer)){
			if(hit.transform.tag == "Player"){
				skin.material.color = Color.red;
				rigidBody.freezeRotation = true;
				targetMarked = true;
				targetPosition = hit.point;
				timer = chargeTime;
				lookAt(hit.point);
			}
		}
	}
}
