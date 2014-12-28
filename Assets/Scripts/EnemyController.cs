using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class EnemyController : Entity {

	public Bullet bullet;
	public GameObject bulletSpawnPoint;

	private Vector3 spawnPoint;
	public float walkAmplitude = 10;

	private int speed = 4;
	private int gravity= 20;

	public float animationSpeed;
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	private int direction = 1;

	private PlayerPhysics playerPhysics;
	private Animator animator;
	private GameManager manager;

	// Use this for initialization
	void Start () {
		spawnPoint = transform.position;;
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (playerPhysics.movementStopped) {
			targetSpeed = 0;
			currentSpeed = 0;
		}
		animationSpeed = IncrementTowards( animationSpeed, Mathf.Abs (targetSpeed), 0);
		animator.SetFloat("Speed", animationSpeed);

		amountToMove.x = speed * direction;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime);

		if (Mathf.Abs(spawnPoint.x - transform.position.x) >= walkAmplitude) {
			direction *= -1;
		}

		transform.eulerAngles = direction > 0 ? Vector3.up * 180: Vector3.zero; 
		direction =  direction > 0 ? 1 : -1;

		
		//Shot
//		if(Input.GetButtonDown("Fire")) {
//			Vector3 bulletSpawnPosition = bulletSpawnPoint.transform.position;
//			bullet = (Instantiate(bullet, bulletSpawnPosition, Quaternion.identity) as Bullet);
//			bullet.Shoot(direction);
//		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			PlayerController playerController = collider.GetComponent<PlayerController>();
			playerController.TakeDamage(1);
			playerController.UpdateHealth();
			Destroy(this.gameObject);
		} else if (collider.tag == "Bullet") {
			Destroy(this.gameObject);
		} else if (collider.tag == "Enemy" || collider.tag == "Obstacle") {
			direction *= -1;
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
