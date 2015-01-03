using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class EnemyController : Entity {

	public Bullet bullet;
	public GameObject gun;
	public GameObject gunPlaceHolder;
	private GameObject carriedGun;
	public int gunType; // 0 - pistol, 1 - submachinegun, 2 - machinegun
	private int gravity= 20;
	
	private Vector2 amountToMove;

	private int direction = 1;

	private PlayerPhysics playerPhysics;
	private Animator animator;
	private GameManager manager;
	private GunController gunController;
	
	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
		gunController = gun.GetComponent<GunController>();
		gun.transform.position = gunPlaceHolder.transform.position;
		carriedGun = Instantiate(gun, gunPlaceHolder.transform.position, Quaternion.Euler(new Vector3(0,-90,0))) as GameObject;
		LoadGunController();
		carriedGun.transform.parent = this.gameObject.transform;
		carriedGun.tag = Tags.EnemyGunTag();
		gunController.isPickUp = true;
		manager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {

		animator.SetFloat(ANIMATOR_SPEED, 0);

		amountToMove.x = 0;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime);

		direction =  transform.position.x < Camera.main.transform.position.x ?  Directions.Left() : Directions.Right();
		transform.eulerAngles = direction < 0 ? Vector3.up * 180: Vector3.zero; 


		if(IsInCamera()) {
			Shoot ();
		} 
	}

	private bool IsInCamera() {
		Bounds cameraBounds = CameraViewPort.OrthographicBounds();
		return Camera.main != null ? (transform.position.x < cameraBounds.max.x && transform.position.x >  cameraBounds.min.x) : false;
	}

	private void Shoot() {
		if(Random.Range(0, 10 * gunType) == 0) {
			gunController.Shoot(-direction, Tags.EnemyBulletTag());
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == Tags.PlayerTag()) {
			PlayerController playerController = collider.GetComponent<PlayerController>();
			playerController.TakeDamage(1);
			Destroy(this.gameObject);
		} else if (collider.tag == Tags.PlayerBulletTag()) {
			Destroy(collider.gameObject);
			TakeDamage(-1);
		} 
	}

	private void LoadGunController() {
		gunController = carriedGun.GetComponent<GunController>();
		if (gunType == GunsConstants.SubmachinegunGunType()) {
			health = 2;
			gunController.MediumEnemyGun();
		} else if(gunType == GunsConstants.MachinegunGunType()) {
			health = 3;
			gunController.SuperEnemyGun();
		} else if(gunType == GunsConstants.BossGunType()) {
			health = 6;
			gunController.SuperEnemyGun();
		}else {
			health = 1;
			gunController.BasicEnemyGun();
		}

	}

	public override void TakeDamage(int damage) {
		health--;
		if (health <= 0) {
			if (gunType == GunsConstants.BossGunType()) {
				GameManager.instance.AddBonusPoints();
				GameManager.instance.FinishGame();
			} else {
				Die();
				GameManager.instance.AddScore();
			}

		}
	}
	
	public override void Die() {
		manager.SpawnGun(gunType, transform.position);
		Destroy(this.gameObject);
	}
}
