using UnityEngine;
using System.Collections;

public class PistolController : GunController {

	// Use this for initialization
	void Start () {
		magazineCapacity = 7;
		ammunition = magazineCapacity;
		magazines = -1; // Inf
		reloadingTime = 5; // seconds
		lastShootTime = 0;
		shootTimeInterval = 3;
		isReloading = false;
	}
	
	void Update() {
		if(isReloading) {
			Reload();
		}
	}
	
	public override void Shoot(int direction, string tag) {
		if (!isReloading) {
			if (Time.time - lastShootTime > shootTimeInterval) {
				Debug.Log (spawnPoint.transform.position);
				Vector3 spawnPosition = spawnPoint.transform.position;
				Bullet firedBullet;
				firedBullet = (Instantiate(bullet, spawnPosition, Quaternion.identity) as Bullet);
				firedBullet.Shoot(direction);
				firedBullet.tag = tag;
				lastShootTime = Time.time;
				ammunition--;
				if (ammunition == 0) {
					isReloading = true;
					startReloadingTime = Time.time;
					Reload();
				}
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

	public override void AddAmmo(int ammunition) {
	}
}
