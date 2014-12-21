using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	// Player handling
	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 32;
	public float jumpHeight = 12;

	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	private PlayerPhysics playerPhysics;


	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
	}

	void Update () {
		if (playerPhysics.movementStopped) {
			targetSpeed = 0;
			currentSpeed = 0;
		}

		targetSpeed = Input.GetAxisRaw("Horizontal") *speed;
		currentSpeed = IncrementPlayerSpeed();

		if(playerPhysics.grounded) {
			amountToMove.y = 0;
			//Jump
			if (Input.GetButtonDown("Jump")) {
				amountToMove.y = jumpHeight;
			}
		}

		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime);
	}

	private float IncrementPlayerSpeed() {
		if (currentSpeed == targetSpeed) {
			return currentSpeed;
		} else {
			float direction = Mathf.Sign (targetSpeed - currentSpeed);
			currentSpeed += acceleration * Time.deltaTime * direction;
			return (direction == Mathf.Sign (targetSpeed - currentSpeed)) ? currentSpeed: targetSpeed;
		}
	}
}
