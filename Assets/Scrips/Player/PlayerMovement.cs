using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float Speed;
	Vector3 movement;
	Rigidbody playerRigidbody;
	float camRayLength = 100f;

	// Use this for initialization
	void Awake(){
		playerRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate(){
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		Move(h, v);
		Turning();
	}

	void Move(float h, float v){
		movement.Set(h, 0f, v);
		movement = movement.normalized * (Speed + PlayerStatus.speedIncreased) *  Time.deltaTime;
		playerRigidbody.MovePosition(transform.position + movement);
	}

	void Turning(){
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit floorHit;
		if(Physics.Raycast(camRay, out floorHit, camRayLength)){
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			playerRigidbody.MoveRotation(newRotation);
		}
	}
}
