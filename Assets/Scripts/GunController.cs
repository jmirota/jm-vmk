using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	public GameObject spawnPoint;
	public Bullet bullet;

	protected int ammunition; //Ammunition left in magazine
	protected int magazineCapacity;
	protected int magazines; //Amout of magazines

	protected float startReloadingTime;
	protected float reloadingTime;
	protected float shootTimeInterval;
	protected float lastShootTime;

	protected bool isReloading;
	public bool isPickUp = false;

	void Update() {
		if(isReloading) {
			Reload();
		}
		
		if (!this.isPickUp) {
			Rotate();
		}
	}

	public virtual void Shoot(int direction, string tag) {
	}

	public virtual void Reload() {
	}

	public virtual void AddAmmo(int ammunition) {
	} 

	public virtual void Rotate () {
		transform.Rotate(new Vector3(0, 50,0) * Time.deltaTime);
	}

	public int Ammo() {
		return ammunition;
	}

	public int Magazines() {
		return magazines;
	}
}
