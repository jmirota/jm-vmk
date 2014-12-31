using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : Entity {

	public GameObject gunPlaceHolder;
	public GameObject pistol;
	public GameObject submachinegun;
	public GameObject machinegun;
	private GameObject carriedGun;
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
	private GameManager manager;
	private GunController gunController;

	private int direction = -1;


	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
		manager = Camera.main.GetComponent<GameManager>();
		manager.SetLives(health);
		carriedGun = Instantiate(pistol, gunPlaceHolder.transform.position, Quaternion.Euler(new Vector3(0,-90,0))) as GameObject;
		gunController = carriedGun.GetComponent<PistolController>();
		carriedGun.transform.parent = this.gameObject.transform;
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
			gunController.Shoot(direction, "PlayerBullet");
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

	public void UpdateHealth() {
		health++;
		if (health >=3) {
			health = 3;
		} 
		manager.SetLives(health);
	}

	private void PickUpPistol(GameObject Pistol) {
		if(carriedGun.tag != "Pistol"){
			Destroy(carriedGun);
			carriedGun = Pistol;
			gunController = carriedGun.GetComponent<PistolController>();
			carriedGun.transform.position = gunPlaceHolder.transform.position;

			carriedGun.transform.parent = this.gameObject.transform;
		}
	}

	private void PickUpSubmachinegun(GameObject Submachinegun) {
		if(carriedGun.tag != "Submachinegun"){
			Destroy(carriedGun);
			carriedGun = Submachinegun;
			gunController = carriedGun.GetComponent<SubmachinegunController>();
			carriedGun.transform.position = gunPlaceHolder.transform.position;
			carriedGun.transform.parent = this.gameObject.transform;
		} else {
			gunController.AddAmmo(10);
			Destroy(Submachinegun);
		}
	}
	
	private void PickUpMachinegun(GameObject Machinegun) {
		if(carriedGun.tag != "Machinegun"){
			Destroy(carriedGun);
			carriedGun = Machinegun;
			gunController = carriedGun.GetComponent<MachinegunController>();
			carriedGun.transform.position = gunPlaceHolder.transform.position;
			carriedGun.transform.parent = this.gameObject.transform;
		} else {
			gunController.AddAmmo(20);
			Destroy(Machinegun);
		}
	}
		
	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "FirstAid") {
			health++;
			manager.SetLives(health);
		} else if (collider.tag == "Checkpoint") {
			manager.SetCheckpoint(collider.transform.position);
		} else if (collider.tag == "Finish") {
			manager.EndLevel();
		} else if (collider.tag == "EnemyBullet") {
			TakeDamage(1);
		} else if (collider.tag == "Pistol") {
			PickUpPistol(collider.gameObject);
		} else if (collider.tag == "Submachinegun") {
			PickUpSubmachinegun(collider.gameObject);
		} else if (collider.tag == "Machinegun") {
			PickUpMachinegun(collider.gameObject);
		}
	}

	public override void TakeDamage(int damage) {
		health--;
		manager.SetLives(health);
		if (health <= 0) {
			Die();
		}
	}
	
	public override void Die() {
		Destroy(this.gameObject);
	}

}
