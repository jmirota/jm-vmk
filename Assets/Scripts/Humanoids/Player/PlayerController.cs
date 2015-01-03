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
	private Gun currentGun;
	
	// Player handling
	public float gravity = 30;
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

	private int direction = 1;
	
	private const int MAX_HEALTH_VALUE = 3;

	//KEYS
	private const string HORIZONTAL_KEYS = "Horizontal";
	private const string JUMP_KEY = "Jump";
	private const string FIRE_KEY = "Fire";

	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
		manager = GameManager.instance;
		PlayerManager.instance.SetLives(health);
		GameHUDManager.instance.SetLives(health);
		currentGun = PlayerManager.instance.CurrentGun();
		ChangeGun();
	}

	void Update () {
		if (playerPhysics.movementStopped) {
			targetSpeed = 0;
			currentSpeed = 0;
		}
		animationSpeed = IncrementTowards( animationSpeed, Mathf.Abs (targetSpeed), accelerationSpeed);
		animator.SetFloat(ANIMATOR_SPEED, animationSpeed);

		//Input
		targetSpeed = Input.GetAxisRaw(HORIZONTAL_KEYS) *runSpeed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, accelerationSpeed);

		if(playerPhysics.grounded) {
			amountToMove.y = 0;
			//Landed
			if (jumping) {
				jumping = false;
				animator.SetBool(ANIMATOR_JUMPING, false);
			}

			//Jump
			if (Input.GetButtonDown(JUMP_KEY)) {
				amountToMove.y = jumpHeight;
				jumping = true;
				animator.SetBool(ANIMATOR_JUMPING, true);
			}
		}

		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime);

		//Face direction
		float moveDirection = Input.GetAxisRaw(HORIZONTAL_KEYS);
		if(moveDirection != 0){
			transform.eulerAngles = moveDirection > 0 ? Vector3.up * 180: Vector3.zero; 
			direction =  moveDirection > 0 ? Directions.Right() : Directions.Left();
		}

		//Shot
		if(Input.GetButtonDown(FIRE_KEY)) {
			gunController.Shoot(direction, Tags.PlayerBulletTag());
			if (!gunController.HasAmmoLeft() && gunController.AmmunitionLeft != GunsConstants.InfinityAmmunition()){
				HandleNoAmmunition();
			} else {
				Gun gun = PlayerManager.instance.CurrentGun();
				gun.Ammunition = gunController.Ammunition;
				gun.AmmunitionLeft = gunController.AmmunitionLeft;
				Debug.Log(gun.Ammunition);
			}
		}
		GameHUDManager.instance.SetAmmoText(gunController.Ammunition, gunController.AmmunitionLeft);
	}

	private void HandleNoAmmunition() {
		ChangeGun();
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
		if (health >= MAX_HEALTH_VALUE) {
			health = MAX_HEALTH_VALUE;
		} 
		PlayerManager.instance.SetLives(health);
		GameHUDManager.instance.SetLives(health);
	}

	private void PickUpPistol() {
		Destroy(carriedGun);
		carriedGun = Instantiate(pistol) as GameObject;
		gunController = carriedGun.GetComponent<GunController>();
		gunController.isPickUp = true;
		gunController.PistolGun();
		SetGunTransform();
	}

	private void PickUpSubmachinegun() {
		PlayerManager.instance.AddSubmachinegunAmmo();
		Destroy(carriedGun);
		carriedGun = Instantiate(submachinegun) as GameObject;
		carriedGun.GetComponent<BoxCollider>().enabled = false;
		gunController = carriedGun.GetComponent<GunController>();
		gunController.isPickUp = true;
		gunController.SubmachineGun();
		SetGunTransform();
	}
	
	private void PickUpMachinegun() {
		PlayerManager.instance.AddMachinegunAmmo();
		Destroy(carriedGun);
		carriedGun = Instantiate(machinegun) as GameObject;
		carriedGun.GetComponent<BoxCollider>().enabled = false;
		gunController = carriedGun.GetComponent<GunController>();
		gunController.isPickUp = true;
		gunController.MachineGun();
		SetGunTransform();
	}
		
	private void SetGunTransform() {
		carriedGun.transform.position = gunPlaceHolder.transform.position;
		carriedGun.transform.parent = this.gameObject.transform;
		carriedGun.transform.rotation = direction == Directions.Right() ? Quaternion.Euler(0,90,0): Quaternion.Euler(0, -90, 0);
	}

	public void ChangeGun() {
		Gun gun = PlayerManager.instance.CurrentGun();
		if (gun.Type == GunType.SubMachinegun) {
			PickUpSubmachinegun();
		} else if (gun.Type == GunType.Machinegun) {
			PickUpMachinegun();
		} else {
			PickUpPistol();
		}

		currentGun = gun;
		gunController.Ammunition = currentGun.Ammunition;
		gunController.AmmunitionLeft = currentGun.AmmunitionLeft;
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == Tags.FirstAidTag()) {
			UpdateHealth();
		}  else if (collider.tag == Tags.FinishTag()) {
			manager.EndLevel();
		} else if (collider.tag == Tags.EnemyBulletTag()) {
			TakeDamage(1);
		} else if (collider.tag == Tags.PistolTag()) {
			Destroy(collider.gameObject);
		} else if (collider.tag == Tags.SubmachinegunTag()) {
			PlayerManager.instance.AddSubmachinegunAmmo();
			Destroy(collider.gameObject);
			ChangeGun();
		} else if (collider.tag == Tags.MachinegunTag()) {
			PlayerManager.instance.AddMachinegunAmmo();
			Destroy (collider.gameObject);
			ChangeGun();
		} else if (collider.tag == Tags.PointsTag()) {
			GameManager.instance.AddScore();
			Destroy (collider.gameObject);
		}
	}

	public override void TakeDamage(int damage) {
		health--;
		PlayerManager.instance.SetLives(health);
		GameHUDManager.instance.SetLives(health);
		if (health <= 0) {
			Die();
		}
	}
	
	public override void Die() {
		int oneLive = 1;
		PlayerManager.instance.SetLives(oneLive);
		GameHUDManager.instance.SetLives(oneLive);
		Destroy(this.gameObject);
	}

	public void SetPlayerLife(int life) {
		health = life;
	}

}
