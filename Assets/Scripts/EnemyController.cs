using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class EnemyController : Entity {

	public Bullet bullet;
	public GameObject gun;
	public GameObject gunPlaceHolder;
	private GameObject carriedGun;

	private Vector3 spawnPoint;
	public float walkAmplitude = 10;

	private int speed = 4;
	private int gravity= 20;

	public float animationSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	private int direction = 1;

	private PlayerPhysics playerPhysics;
	private Animator animator;
	private GameManager manager;
	private GunController gunController;

	Camera cam;
	Plane[] planes;

	// Use this for initialization
	void Start () {
		spawnPoint = transform.position;;
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
		gunController = gun.GetComponent<PistolController>();
		gun.transform.position = gunPlaceHolder.transform.position;
		carriedGun = Instantiate(gun, gunPlaceHolder.transform.position, Quaternion.Euler(new Vector3(0,-90,0))) as GameObject;
		gunController = carriedGun.GetComponent<PistolController>();
		carriedGun.transform.parent = this.gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerPhysics.movementStopped) {
			targetSpeed = 0;
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


		if(Mathf.Abs(transform.position.x - Camera.main.transform.position.x) <= 10) {
			direction =  Mathf.Abs(transform.position.x) < Mathf.Abs(Camera.main.transform.position.x) ?  -1 : 1;
			speed = 0;
			targetSpeed = 0;
			animationSpeed = 0;
			gunController.Shoot(direction, "EnemyBullet");
		} else {
			speed = 4;
			animationSpeed = 2;
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			PlayerController playerController = collider.GetComponent<PlayerController>();
			playerController.TakeDamage(1);
			Destroy(this.gameObject);
		} else if (collider.tag == "PlayerBullet") {
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
