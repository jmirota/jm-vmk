using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public int health;

	public void TakeDamage(int damage) {
		health -= damage;

		if (health <= 0) {
			Die();
		}
	}

	public void Die() {
		Destroy(this.gameObject);
	}

}
