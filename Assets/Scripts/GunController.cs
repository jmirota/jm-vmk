using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	public GameObject spawnPoint;
	public Bullet bullet;

	protected static int ammunition; //Ammunition left in magazine
	protected int magazineCapacity;
	protected int magazines; //Amout of magazines

	protected float startReloadingTime;
	protected float reloadingTime;
	protected float shootTimeInterval;
	protected float lastShootTime;

	protected bool isReloading;

	public virtual void Shoot(int direction, string tag) {
	}

	public virtual void Reload() {
	}

	public virtual void AddAmmo(int ammunition) {
	}
}
