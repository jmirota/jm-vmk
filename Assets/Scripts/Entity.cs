using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	protected int health = 1;

	public virtual void TakeDamage(int damage) {
	}

	public virtual void Die() {
	}

}
