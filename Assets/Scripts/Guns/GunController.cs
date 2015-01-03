using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {
	private enum GunOwner{Player, Enemy};

	public GameObject spawnPoint;
	public Bullet bullet;
	public new ParticleSystem particleSystem;
	
	private int ammunition; //Ammunition left in magazine
	public int Ammunition {
		get
		{
			return ammunition;
		}
		set
		{
			ammunition = value;
		}
	}

	private int ammunitionLeft;
	public int AmmunitionLeft {
		get
		{
			return ammunitionLeft;
		}
		set
		{
			ammunitionLeft = value;
		}
	}

	private int magazineCapacity;

	private float startReloadingTime;
	private float reloadingTime;
	private float shootTimeInterval;
	private float lastShootTime;

	private bool isReloading;
	public bool isPickUp = false;
	private GunOwner owner;

	public AudioClip audioClip;

	void Start () {
		isReloading = false;
		if (owner == GunOwner.Player) {
			GameHUDManager.instance.HideReloadingText();
		}

		if (Ammunition == GunsConstants.EmptyMagazine()) {
			isReloading = true;
			startReloadingTime = Time.time;
			Reload();
		}

		audio.clip = audioClip;
	}
	
	void Update() {
		if(isReloading) {
			Reload();
		}
		
		if (!this.isPickUp) {
			Rotate();
			particleSystem.enableEmission = true;
		} else {
			particleSystem.enableEmission = false;
		}
	}

	public void BasicEnemyGun () {
		ammunition = 7;
		ammunitionLeft = GunsConstants.InfinityAmmunition();
		reloadingTime = 5; // seconds
		lastShootTime = 0;
		shootTimeInterval = 1.5f;
		isReloading = false;
		reloadingTime = 0.1f;
		owner = GunOwner.Enemy;
	}
	
	public void MediumEnemyGun () {
		magazineCapacity = 20;
		ammunitionLeft = GunsConstants.InfinityAmmunition();
		reloadingTime = 8; // seconds
		lastShootTime = 0;
		shootTimeInterval = 1.2f;
		isReloading = false;
		reloadingTime = 0.1f;
		owner = GunOwner.Enemy;
	}

	public void SuperEnemyGun() {
		magazineCapacity = 30;
		ammunition = magazineCapacity;
		ammunitionLeft = GunsConstants.InfinityAmmunition();
		reloadingTime = 5; // seconds
		lastShootTime = 0;
		shootTimeInterval = 0.9f;
		isReloading = false;
		reloadingTime = 0.1f;
		owner = GunOwner.Enemy;
	}

	public void PistolGun () {
		magazineCapacity = 7;
		ammunition = magazineCapacity;
		ammunitionLeft = GunsConstants.InfinityAmmunition();
		reloadingTime = 1.6f; // seconds
		lastShootTime = 0;
		shootTimeInterval = 0.7f;
		isReloading = false;
		owner = GunOwner.Player;
	}
	
	public void SubmachineGun () {
		magazineCapacity = 20;
		ammunitionLeft = 40;
		ammunition = magazineCapacity;
		reloadingTime = 2; // seconds
		lastShootTime = 0;
		shootTimeInterval = 0.2f;
		isReloading = false;
		owner = GunOwner.Player;
	}
	
	public void MachineGun() {
		magazineCapacity = 30;
		ammunitionLeft = 90;
		ammunition = magazineCapacity;
		reloadingTime = 2.1f; // seconds
		lastShootTime = 0;
		shootTimeInterval = 0.06f;
		isReloading = false;
		owner = GunOwner.Player;
	}


	public void Shoot(int direction, string tag) {
		if (!isReloading) {
			if (CanShoot()) {
				Vector3 spawnPosition = spawnPoint.transform.position;
				Bullet firedBullet;
				firedBullet = (Instantiate(bullet, spawnPosition, Quaternion.identity) as Bullet);
				firedBullet.Shoot(direction);
				firedBullet.tag = tag;
				ammunition--;
				lastShootTime = Time.time;
				audio.Play();
				if (ammunition == GunsConstants.EmptyMagazine()) {
					isReloading = true;
					startReloadingTime = Time.time;
					if (owner == GunOwner.Player) {
						GameHUDManager.instance.ShowReloadingText();
					}
					Reload();
				} 
			}
		}
	}

	private bool CanShoot() {
		int numberOfAmmunition = ammunition;
		if (ammunitionLeft != GunsConstants.InfinityAmmunition()) {
			numberOfAmmunition += ammunitionLeft;
		}
		return Time.time - lastShootTime > shootTimeInterval && numberOfAmmunition != GunsConstants.EmptyMagazine();
	}
	
	public void Reload() {
		if ( Time.time - startReloadingTime > reloadingTime) {
			ammunition = magazineCapacity;
			if (ammunitionLeft != GunsConstants.InfinityAmmunition()) {
				ammunitionLeft -= ammunition;
			}
			isReloading = false;
			if (owner == GunOwner.Player) {
				GameHUDManager.instance.HideReloadingText();
				GameHUDManager.instance.SetAmmoText(Ammunition, AmmunitionLeft);
			}
		}
	}
	
	public void AddAmmo(int ammo){
		if (ammunitionLeft != GunsConstants.InfinityAmmunition()) {
			ammunitionLeft += ammo;
		}
	}

	public void Rotate () {
		transform.Rotate(new Vector3(0, 50,0) * Time.deltaTime);
	}
	
	public bool HasAmmoLeft() {
		int numberOfAmmunition = ammunition;
		if (ammunitionLeft != GunsConstants.InfinityAmmunition()) {
			numberOfAmmunition += ammunitionLeft;
		}
		return numberOfAmmunition == GunsConstants.EmptyMagazine() ? false : true;
	}
}
