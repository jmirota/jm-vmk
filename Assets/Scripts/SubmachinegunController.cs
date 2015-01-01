using UnityEngine;
using System.Collections;

public class SubmachinegunController : GunController {
	
	// Use this for initialization
	void Start () {
		magazines = 2;
		magazineCapacity = 20;
		ammunition = magazineCapacity * magazines;
		reloadingTime = 8; // seconds
		lastShootTime = 0;
		shootTimeInterval = 2;
		isReloading = false;
	}

	public override void Shoot(int direction, string tag) {
		if (!isReloading) {
			if (Time.time - lastShootTime > shootTimeInterval) {
				Vector3 spawnPosition = spawnPoint.transform.position;
				Bullet firedBullet;
				firedBullet = (Instantiate(bullet, spawnPosition, Quaternion.identity) as Bullet);
				firedBullet.Shoot(direction);
				firedBullet.tag = tag;
				ammunition--;
				magazines = ammunition / magazineCapacity;
				if (ammunition % magazineCapacity == 0) {
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

	public override void AddAmmo(int ammo) {
		ammunition += ammo;
	}
}
