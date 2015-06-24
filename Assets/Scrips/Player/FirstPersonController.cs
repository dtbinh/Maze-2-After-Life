using UnityEngine;

public class FirstPersonController : MonoBehaviour {
	public float baseSpeed = 6.0f;
	public float currentSpeed;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
	private float vSpeed;
    private Vector3 moveDirection = Vector3.zero;

	float verticalRotation;
	const float UP_DOWN_RANGE = 80f;

	public static CharacterController controller;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
		currentSpeed = baseSpeed;
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		// Camera rotate up and down
		float rotLeftRight = Input.GetAxis("Mouse X") * GameMaster.mouseSensitivity;
		transform.Rotate(0,rotLeftRight,0);
		// Camera rotate left and right
		verticalRotation -= Input.GetAxis("Mouse Y") * GameMaster.mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -UP_DOWN_RANGE, UP_DOWN_RANGE);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation,0,0);

		//Feed moveDirection with input.
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		//Multiply it by speed.
		moveDirection *= currentSpeed;

        // is the controller on the ground?
        if (controller.isGrounded) {
			vSpeed = 0;
            //Jumping
            if (Input.GetButton("Jump"))
                vSpeed = jumpSpeed;
        }
		vSpeed -= gravity * Time.deltaTime;

        //Applying gravity to the controller
		moveDirection.y = vSpeed;
        //Making the character move
        controller.Move(moveDirection * Time.deltaTime);
    }
}
