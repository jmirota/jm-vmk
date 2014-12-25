using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public float health;

	public void TakeDamage(float damage) {
		health -= damage;

		if (health <= 0) {
			Die();
		}
	}

	public void Die() {
		Destroy(this.gameObject);
	}

}
