using UnityEngine;
using System.Collections;

public class MachinegunController : GunController {

	// Use this for initialization
	void Start () {
		magazineCapacity = 30;
		ammunition = magazineCapacity;
		magazines = -1; // Inf
		reloadingTime = 5; // seconds
		lastShootTime = 0;
		shootTimeInterval = 1;
		isReloading = false;
	}
	
	void Update() {
		if(isReloading) {
			Reload();
		}
	}
	
	public override void Shoot(int direction, string tag) {
		if (!isReloading) {
			Vector3 spawnPosition = spawnPoint.transform.position;
			Bullet firedBullet;
			firedBullet = (Instantiate(bullet, spawnPosition, Quaternion.identity) as Bullet);
			firedBullet.Shoot(direction);
			firedBullet.tag = tag;
			ammunition--;
			magazines = ammunition / magazineCapacity;
			Debug.Log (ammunition);
			if (ammunition % magazineCapacity == 0) {
				isReloading = true;
				startReloadingTime = Time.time;
				Reload();
			}
		}
	}
	
	public override void Reload() {
		Debug.Log (Time.time - startReloadingTime);
		if ( Time.time - startReloadingTime > reloadingTime) {
			ammunition = magazineCapacity;
			isReloading = false;
		}
	}

	public override void AddAmmo(int ammo){
		ammunition += ammo;
	}
}
