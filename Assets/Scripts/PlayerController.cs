using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : Entity {

	public Bullet bullet;
	public GameObject spawnPoint;


	// Player handling
	public float gravity = 20;
	public float runSpeed = 3;
	public float accelerationSpeed = 11;
	public float jumpHeight = 12;

	public float animationSpeed;
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	//States
	private bool jumping;

	private PlayerPhysics playerPhysics;
	private Animator animator;
	private int direction = 1;

	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
	}

	void Update () {
		if (playerPhysics.movementStopped) {
			targetSpeed = 0;
			currentSpeed = 0;
		}
		animationSpeed = IncrementTowards( animationSpeed, Mathf.Abs (targetSpeed), accelerationSpeed);
		animator.SetFloat("Speed", animationSpeed);

		//Input
		targetSpeed = Input.GetAxisRaw("Horizontal") *runSpeed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, accelerationSpeed);

		if(playerPhysics.grounded) {
			amountToMove.y = 0;
			//Landed
			if (jumping) {
				jumping = false;
				animator.SetBool("Jumping", false);
			}

			//Jump
			if (Input.GetButtonDown("Jump")) {
				amountToMove.y = jumpHeight;
				jumping = true;
				animator.SetBool("Jumping", true);
			}
		}

		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime);

		//Face direction
		float moveDirection = Input.GetAxisRaw("Horizontal");
		if(moveDirection != 0){
			transform.eulerAngles = moveDirection > 0 ? Vector3.up * 180: Vector3.zero; 
			direction =  moveDirection > 0 ? 1 : -1;
		}

		//Shot
		if(Input.GetButtonDown("Fire")) {
			Vector3 spawnPosition = spawnPoint.transform.position;
			bullet = (Instantiate(bullet, spawnPosition, Quaternion.identity) as Bullet);
			bullet.Shoot(direction);
		}
	}

	private float IncrementTowards(float current, float target, float acceleration) {
		if (current == target) {
			return current;
		} else {
			float direction = Mathf.Sign (targetSpeed - current);
			current += acceleration * Time.deltaTime * direction;
			return (direction == Mathf.Sign (targetSpeed - current)) ? current: target;
		}
	}
}
