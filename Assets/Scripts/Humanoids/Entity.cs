using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	protected int health = 1;

	//ANIMATOR FLAGS
	protected const string ANIMATOR_SPEED = "Speed";
	protected const string ANIMATOR_JUMPING = "Jumping";

	public virtual void TakeDamage(int damage) {
	}

	public virtual void Die() {
	}

}
