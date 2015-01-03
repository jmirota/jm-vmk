using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	private PlayerController playerController;
	private GameCamera gameCamera;
	private GameObject currentPlayer;

	private GameObject GameCamera;
	public GameObject player;
	private static int playerLives = 1;

	private Vector3 checkpoint;

	private static PlayerManager _instance;
	
	public static PlayerManager instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<PlayerManager>();
				
				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}
			
			return _instance;
		}
	}
	
	private static Gun[] guns;
	
	// Use this for initialization
	void Start () {
		GameCamera = GameObject.FindGameObjectWithTag(Tags.MainCameraTag()) as GameObject;
		gameCamera = GameCamera.GetComponent<GameCamera>();
		if(GameObject.FindGameObjectWithTag(Tags.SpawnTag())) {
			if (guns == null || guns.Length == 0){
				CreateGuns();
			}
			checkpoint = GameObject.FindGameObjectWithTag(Tags.SpawnTag()).transform.position;
			SpawnPlayer(checkpoint);
			playerController = currentPlayer.GetComponent<PlayerController>();
			playerController.SetPlayerLife(playerLives);
		}
	}
	

	private void SpawnPlayer(Vector3 spawnPosition) {
		currentPlayer = (Instantiate(player, spawnPosition, Quaternion.Euler(new Vector3(0,180,0))) as GameObject);
		gameCamera.SetTarget(currentPlayer.transform);
	}

	public void SetLives(int lives) {
		playerLives = lives;
		if (lives == 0) {
			Application.LoadLevel("GameOver");
		}
	}

	public void AddSubmachinegunAmmo() {
		if (guns[GunsConstants.SubmachinegunGunType()].AmmunitionLeft == GunsConstants.EmptyMagazine() && guns[GunsConstants.SubmachinegunGunType()].Ammunition == GunsConstants.EmptyMagazine() ) {
			guns[GunsConstants.SubmachinegunGunType()].Ammunition = GunsConstants.SubmachinegunMagazineCapacity();
			guns[GunsConstants.SubmachinegunGunType()].AmmunitionLeft = GunsConstants.SubmachineStartAmmunitionLeftQuantity();
		} else if (guns[GunsConstants.SubmachinegunGunType()].Ammunition == GunsConstants.EmptyMagazine()) {
			guns[GunsConstants.SubmachinegunGunType()].Ammunition += GunsConstants.SubmachinegunMagazineCapacity();
			guns[GunsConstants.SubmachinegunGunType()].AmmunitionLeft += GunsConstants.SubmachinegunMagazineCapacity();
		} else {
			guns[GunsConstants.SubmachinegunGunType()].AmmunitionLeft += GunsConstants.SubmachineStartAmmunitionLeftQuantity();
		}

		if (guns[GunsConstants.SubmachinegunGunType()].Ammunition > GunsConstants.SubmachinegunMagazineCapacity()) {
			guns[GunsConstants.SubmachinegunGunType()].Ammunition = GunsConstants.SubmachinegunMagazineCapacity();
		}

		if (guns[GunsConstants.SubmachinegunGunType()].AmmunitionLeft > GunsConstants.SubmachinegunTotalAmmunitionCapacity()) {
			guns[GunsConstants.SubmachinegunGunType()].AmmunitionLeft = GunsConstants.SubmachinegunTotalAmmunitionCapacity();
		}
	}

	public void AddMachinegunAmmo() {
		if (guns[GunsConstants.MachinegunGunType()].AmmunitionLeft == GunsConstants.EmptyMagazine() && guns[GunsConstants.MachinegunGunType()].Ammunition == GunsConstants.EmptyMagazine() ) {
			guns[GunsConstants.MachinegunGunType()].Ammunition = GunsConstants.MachinegunMagazineCapacity();
			guns[GunsConstants.MachinegunGunType()].AmmunitionLeft = GunsConstants.MachineStartAmmunitionLeftQuantity();
		} else if (guns[GunsConstants.MachinegunGunType()].Ammunition == GunsConstants.EmptyMagazine()) {
			guns[GunsConstants.MachinegunGunType()].Ammunition += GunsConstants.MachinegunMagazineCapacity();
			guns[GunsConstants.MachinegunGunType()].AmmunitionLeft += GunsConstants.MachinegunMagazineCapacity();
		} else {
			guns[GunsConstants.MachinegunGunType()].AmmunitionLeft += GunsConstants.MachineStartAmmunitionLeftQuantity();
		}
		
		if (guns[GunsConstants.MachinegunGunType()].Ammunition > GunsConstants.MachinegunMagazineCapacity()) {
			guns[GunsConstants.MachinegunGunType()].Ammunition = GunsConstants.MachinegunMagazineCapacity();
		}
		
		if (guns[GunsConstants.MachinegunGunType()].AmmunitionLeft > GunsConstants.MachinegunTotalAmmunitionCapacity()) {
			guns[GunsConstants.MachinegunGunType()].AmmunitionLeft = GunsConstants.MachinegunTotalAmmunitionCapacity();
		}
	}

	public Gun CurrentGun () {
		if (guns[GunsConstants.MachinegunGunType()].AmmunitionLeft + guns[GunsConstants.MachinegunGunType()].Ammunition > GunsConstants.EmptyMagazine()) {
			return guns[GunsConstants.MachinegunGunType()];
		} else if (guns[GunsConstants.SubmachinegunGunType()].AmmunitionLeft + guns[GunsConstants.SubmachinegunGunType()].Ammunition > GunsConstants.EmptyMagazine()) {
			return guns[GunsConstants.SubmachinegunGunType()];
		} else {
			return guns[GunsConstants.PistolGunType()];
		}
	}

	public void Shot () {
		Gun currentGun = CurrentGun();
		currentGun.Ammunition -= 1;
		currentGun.AmmunitionLeft -= 1;
	}

	private void CreateGuns() {
		Gun Pistol = new Gun();
		Pistol.Type = GunType.Pistol;
		Pistol.AmmunitionLeft = GunsConstants.InfinityAmmunition();;
		Pistol.Ammunition = 7;

		Gun Submachinegun = new Gun();
		Submachinegun.Type = GunType.SubMachinegun;
		Submachinegun.AmmunitionLeft = GunsConstants.EmptyMagazine();
		Submachinegun.Ammunition = GunsConstants.EmptyMagazine();

		Gun Machinegun = new Gun();
		Machinegun.Type = GunType.Machinegun;
		Machinegun.AmmunitionLeft = GunsConstants.EmptyMagazine();
		Machinegun.Ammunition = GunsConstants.EmptyMagazine();

		guns = new Gun[3];
		guns[GunsConstants.PistolGunType()] = Pistol;
		guns[GunsConstants.SubmachinegunGunType()] = Submachinegun;
		guns[GunsConstants.MachinegunGunType()] = Machinegun;
	}

	public void Restart() {
		if (guns == null || guns.Length == 0){
			CreateGuns();
		}
		guns[GunsConstants.PistolGunType()].Ammunition = 7;
		guns[GunsConstants.SubmachinegunGunType()].AmmunitionLeft = GunsConstants.EmptyMagazine();
		guns[GunsConstants.SubmachinegunGunType()].Ammunition = GunsConstants.EmptyMagazine();
		guns[GunsConstants.MachinegunGunType()].AmmunitionLeft = GunsConstants.EmptyMagazine();
		guns[GunsConstants.MachinegunGunType()].Ammunition = GunsConstants.EmptyMagazine();

		playerLives = 1;
	}
}
